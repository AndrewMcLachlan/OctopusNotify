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

        public event EventHandler<DeploymentEventArgs> DeploymentSucceeded;
        public event EventHandler<DeploymentEventArgs> DeploymentFailed;
        public event EventHandler<DeploymentEventArgs> DeploymentFailedNew;
        public event EventHandler<DeploymentEventArgs> DeploymentFixed;

        public event EventHandler<DeploymentEventArgs> DeploymentManualStep;
        public event EventHandler<DeploymentEventArgs> DeploymentGuidedFailure;

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

        private DashboardResource _previousDashboard;
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
            _pollingTimer.Interval = interval;
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

                //if (_previousDashboard == null) _previousDashboard = dashboard;

                Log.Debug("Got dashboard");

                if (_brokenConnection)
                {
                    _brokenConnection = false;
                    OnConnectionRestored();
                }

                FixedDeployments(dashboard);
                NewFailedDeployments(dashboard);
                FailedDeployments(dashboard);
                CompletedDeployments(dashboard);
                ManualStepDeployments(dashboard);
                GuidedFailureDeployments(dashboard);

                if (dashboard.Items.All(i => i.IsCurrent && !ErrorStates.Contains(i.State)))
                {
                    OnErrorsCleared();
                }
                else if (dashboard.Items.Any(i => i.IsCurrent && ErrorStates.Contains(i.State)))
                {
                    OnErrorsFound();
                }

                _lastElapsed = now;

                _failedBuilds = _failedBuilds.Intersect(dashboard.Items.Where(i => i.State != TaskState.Success), new DashboardItemResourceComparer())
                                             .Union(dashboard.Items.Where(i => ErrorStates.Contains(i.State)))
                                             .ToList();
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

        private void FixedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              i.State == TaskState.Success &&
                              !i.HasWarningsOrErrors && p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            if (items.Any())
            {
                OnDeploymentFixed(new DeploymentEventArgs
                {
                    Items = items.ToList()
                });
            }
        }

        private void CompletedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              i.State == TaskState.Success &&
                              !i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            // Get any first-time builds
            items = items.Union(dashboard.Items.Where(i => i.State == TaskState.Success && !i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !_failedBuilds.Any(p => i.ProjectId == p.ProjectId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard)));

            if (items.Any())
            {
                OnDeploymentSucceeded(new DeploymentEventArgs
                {
                    Items = items.ToList()
                });
            }
        }

        private void NewFailedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              ErrorStates.Contains(i.State) &&
                              i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            // Get any first-time builds
            items = items.Union(dashboard.Items.Where(i => ErrorStates.Contains(i.State) && i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !_failedBuilds.Any(p => i.ProjectId == p.ProjectId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard)));

            if (items.Any())
            {
                OnDeploymentFailedNew(new DeploymentEventArgs
                {
                    Items = items.ToList()
                });
            }
        }

        private void FailedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in _failedBuilds
                        where i.ProjectId == p.ProjectId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              ErrorStates.Contains(i.State) &&
                              i.HasWarningsOrErrors && p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            if (items.Any())
            {
                OnDeploymentFailed(new DeploymentEventArgs
                {
                    Items = items.ToList()
                });
            }
        }

        private void GuidedFailureDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        where i.HasWarningsOrErrors &&
                              i.HasPendingInterruptions &&
                              !i.IsCompleted
                        select i.ToDeploymentResult(dashboard);

            if (items.Any())
            {
                var newItems = new List<DeploymentResult>();

                foreach (var item in items)
                {
                    var interruptions = _repository.Interruptions.List(regardingDocumentId:item.TaskId, pendingOnly: true);
                    if (interruptions.Items.Any(i => i.Created >= _lastElapsed))
                    {
                        newItems.Add(item);
                    }
                }

                if (newItems.Any())
                {
                    OnDeploymentGuidedFailure(new DeploymentEventArgs
                    {
                        Items = newItems,
                    });
                }
            }
        }

        private void ManualStepDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        where !i.HasWarningsOrErrors &&
                              i.HasPendingInterruptions &&
                              !i.IsCompleted
                        select i.ToDeploymentResult(dashboard);

            if (items.Any())
            {
                var newItems = new List<DeploymentResult>();

                foreach (var item in items)
                {
                    var interruptions = _repository.Interruptions.List(regardingDocumentId: item.TaskId, pendingOnly: true);
                    if (interruptions.Items.Any(i => i.Created >= _lastElapsed))
                    {
                        newItems.Add(item);
                    }
                }

                if (newItems.Any())
                {
                    OnDeploymentManualStep(new DeploymentEventArgs
                    {
                        Items = newItems,
                    });
                }
            }
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

        private void OnDeploymentSucceeded(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Succeeded, {count} items", e.Items.Count);
            DeploymentSucceeded?.Invoke(this, e);
        }

        private void OnDeploymentFailed(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Failed, {count} items", e.Items.Count);
            DeploymentFailed?.Invoke(this, e);
        }

        private void OnDeploymentFailedNew(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Failed (new), {count} items", e.Items.Count);
            DeploymentFailedNew?.Invoke(this, e);
        }

        private void OnDeploymentFixed(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Fixed, {count} items", e.Items.Count);
            DeploymentFixed?.Invoke(this, e);
        }

        private void OnDeploymentManualStep(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Manual Step, {count} items", e.Items.Count);
            DeploymentManualStep?.Invoke(this, e);
        }

        private void OnDeploymentGuidedFailure(DeploymentEventArgs e)
        {
            Log.Debug("On Deployment Guided Failure, {count} items", e.Items.Count);
            DeploymentGuidedFailure?.Invoke(this, e);
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
