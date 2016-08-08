using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Octopus.Client;
using Octopus.Client.Exceptions;
using Octopus.Client.Model;
using OctopusNotify.Model;
using OctopusNotify.Utilities;
using Serilog;

namespace OctopusNotify
{
    public class OctopusAdapter : IDeploymentRepositoryAdapter, IDisposable
    {
        #region Events
        public event EventHandler ConnectionError;
        public event EventHandler ConnectionRestored;

        public event EventHandler<DeploymentEventArgs> DeploymentsChanged;
        public event EventHandler<DeploymentSummaryEventArgs> DeploymentSummaryChanged;

        public event EventHandler ErrorsCleared;
        public event EventHandler ErrorsFound;
        #endregion

        #region Fields
        private static readonly TaskState[] ErrorStates = new[] { TaskState.Failed, TaskState.TimedOut };

        private double _pollingInterval;
        private Timer _pollingTimer = new Timer();
        private DateTime _lastElapsed = DateTime.Now;

        private bool _brokenConnection;

        private IOctopusRepository _repository;

        private List<DashboardItemResource> _failedBuilds = new List<DashboardItemResource>();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public OctopusAdapter(IOctopusRepository repository, double interval)
        {
            _repository = repository;

            _pollingInterval = interval;
            _pollingTimer.Elapsed += PollingTimer_Elapsed;
        }
        #endregion

        #region Public Methods
        public void StartPolling()
        {
            StartPolling(_pollingInterval);
        }

        public void StartPolling(double interval)
        {
            _pollingTimer.Interval = Math.Max(interval, 1000);
            _pollingTimer.Start();
            Task.Factory.StartNew(() => Poll());
        }

        public void StopPolling()
        {
            _pollingTimer.Stop();
        }

        public void Poll()
        {
            Log.Debug("Polling...");

            DateTime now = DateTime.Now;

            try
            {
                var dashboard = _repository.Dashboards.GetDashboard();

                Log.Debug("Got dashboard");

                if (_brokenConnection)
                {
                    _brokenConnection = false;
                    OnConnectionRestored();
                }

                List<DeploymentResult> results = new List<DeploymentResult>();

                results.AddRange(FixedDeployments(dashboard));
                results.AddRange(NewFailedDeployments(dashboard));
                results.AddRange(FailedDeployments(dashboard));
                results.AddRange(CompletedDeployments(dashboard));
                results.AddRange(ManualStepDeployments(dashboard));
                results.AddRange(GuidedFailureDeployments(dashboard));

                OnDeployment(new DeploymentEventArgs(results.OrderBy(r => r.EventTime)));

                _lastElapsed = now;

                _failedBuilds = _failedBuilds.Intersect(dashboard.Items.Where(i => i.State != TaskState.Success), new DashboardItemResourceComparer())
                                             .Union(dashboard.Items.Where(i => ErrorStates.Contains(i.State)), new DashboardItemResourceComparer())
                                             .ToList();

                if (_failedBuilds.Any())
                {
                    OnErrorsFound();
                }
                else
                {
                    OnErrorsCleared();
                }

                OnDeploymentSummaryChanged(new DeploymentSummaryEventArgs(GetDeploymentSummary(dashboard)));
            }
            catch
            {
                if (!_brokenConnection)
                {
                    OnConnectionError();
                }
                _brokenConnection = true;
                return;
            }
        }
        #endregion

        #region Private Methods
        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                _pollingTimer.Stop();
            }
            Poll();
            if (Debugger.IsAttached)
            {
                _pollingTimer.Start();
            }
        }

        private IEnumerable<DeploymentResult> FixedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              i.State == TaskState.Success &&
                              !i.HasWarningsOrErrors && p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard, DeploymentStatus.Fixed);

            return items;
        }

        private IEnumerable<DeploymentResult> CompletedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              i.State == TaskState.Success &&
                              !i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard, DeploymentStatus.Success);

            // Get any first-time builds
            items = items.Union(dashboard.Items.Where(i => i.State == TaskState.Success && !i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !_failedBuilds.Any(p => i.ProjectId == p.ProjectId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard, DeploymentStatus.Success)));

            return items;
        }

        private IEnumerable<DeploymentResult> NewFailedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              ErrorStates.Contains(i.State) &&
                              i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard, i.State.ToDeploymentStatus(100));

            // Get any first-time builds
            items = items.Union(dashboard.Items.Where(i => ErrorStates.Contains(i.State) && i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !_failedBuilds.Any(p => i.ProjectId == p.ProjectId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard, DeploymentStatus.FailedNew)));

            return items;
        }

        private IEnumerable<DeploymentResult> FailedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              ErrorStates.Contains(i.State) &&
                              i.HasWarningsOrErrors && p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard, i.State.ToDeploymentStatus());

            return items;
        }

        private IEnumerable<DeploymentResult> GuidedFailureDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        where i.HasWarningsOrErrors &&
                              i.HasPendingInterruptions &&
                              !i.IsCompleted
                        select i.ToDeploymentResult(dashboard, DeploymentStatus.GuidedFailure);

            if (items.Any())
            {
                var currentItems = new List<DeploymentResult>();

                foreach (var item in items)
                {
                    var interruptions = _repository.Interruptions.List(regardingDocumentId:item.TaskId, pendingOnly: true);
                    if (interruptions.Items.Any(i => i.Created >= _lastElapsed))
                    {
                        item.EventTime = interruptions.Items.Where(i => i.Created >= _lastElapsed).OrderByDescending(i => i.Created).Select(i => i.Created).First();
                        currentItems.Add(item);
                    }
                }

                return currentItems;
            }

            return items;
        }

        private IEnumerable<DeploymentResult> ManualStepDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        where !i.HasWarningsOrErrors &&
                              i.HasPendingInterruptions &&
                              !i.IsCompleted
                        select i.ToDeploymentResult(dashboard, DeploymentStatus.ManualStep);

            if (items.Any())
            {
                var currentItems = new List<DeploymentResult>();

                foreach (var item in items)
                {
                    var interruptions = _repository.Interruptions.List(regardingDocumentId: item.TaskId, pendingOnly: true);
                    if (interruptions.Items.Any(i => i.Created >= _lastElapsed))
                    {
                        item.EventTime = interruptions.Items.Where(i => i.Created >= _lastElapsed).OrderByDescending(i => i.Created).Select(i => i.Created).First();
                        currentItems.Add(item);
                    }
                }

                return currentItems;
            }

            return items;
        }

        private Dictionary<DeploymentStatus, int> GetDeploymentSummary(DashboardResource dashboard)
        {
            var results = dashboard.Items.Where(i => i.IsCurrent && (i.IsCompleted || !i.HasPendingInterruptions)).GroupBy(i => i.State).ToDictionary(g => g.Key.ToDeploymentStatus(), g => g.Count());

            int manualSteps = dashboard.Items.Count(i => i.IsCurrent && !i.IsCompleted && i.HasPendingInterruptions && !i.HasWarningsOrErrors);
            if (manualSteps > 0)
            {
                results.Add(DeploymentStatus.ManualStep, manualSteps);
            }

            int guidedFailures = dashboard.Items.Count(i => i.IsCurrent && !i.IsCompleted && i.HasPendingInterruptions && i.HasWarningsOrErrors);
            if (guidedFailures > 0)
            {
                results.Add(DeploymentStatus.GuidedFailure, guidedFailures);
            }

            return results;
        }
        #endregion

        #region Event Callers
        private void OnConnectionError()
        {
            Log.Debug("On Connection Error");
            ConnectionError?.Invoke(this, EventArgs.Empty);
        }

        private void OnConnectionRestored()
        {
            Log.Debug("On Connection Restored");
            ConnectionRestored?.Invoke(this, EventArgs.Empty);
        }

        private void OnErrorsCleared()
        {
            Log.Debug("On Errors Cleared");
            ErrorsCleared?.Invoke(this, EventArgs.Empty);
        }

        private void OnErrorsFound()
        {
            Log.Debug("On Errors Found");
            ErrorsFound?.Invoke(this, EventArgs.Empty);
        }

        private void OnDeployment(DeploymentEventArgs e)
        {
            DeploymentsChanged?.Invoke(this, e);
        }

        private void OnDeploymentSummaryChanged(DeploymentSummaryEventArgs e)
        {
            DeploymentSummaryChanged?.Invoke(this, e);
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pollingTimer.Dispose();
            }
            _pollingTimer = null;
        }

        ~OctopusAdapter()
        {
            Dispose(false);
        }
        #endregion
    }
}
