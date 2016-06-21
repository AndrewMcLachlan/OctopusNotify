using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public event EventHandler ErrorsCleared;
        public event EventHandler ErrorsFound;
        #endregion

        #region Fields
        private double _pollingInterval;
        private Timer _pollingTimer = new Timer();
        private DateTime _lastElapsed = DateTime.Now;

        private bool _brokenConnection;

        private IOctopusRepository _repository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public OctopusAdapter(IOctopusRepository repository, double interval)
        {
            _repository = repository;

            try
            {
                var dashboard = _repository.Dashboards.GetDashboard();
            }
            catch (OctopusSecurityException osex)
            {
                throw new RepositoryException("Security error connecting to deployment repository", osex);
            }

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
            Poll();
        }

        public void StopPolling()
        {
            _pollingTimer.Stop();
        }
        #endregion

        #region Private Methods
        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Poll();
        }

        private void Poll()
        {
            Log.Debug("Polling...");

            DateTime now = DateTime.Now;

            try
            {
                if (Debugger.IsAttached)
                {
                    _pollingTimer.Stop();
                }

                var dashboard = _repository.Dashboards.GetDashboard();

                Log.Debug("Got dashboard");

                if (_brokenConnection)
                {
                    _brokenConnection = false;
                    OnConnectionRestored();
                }

                FixedDeployments(dashboard);
                NewBrokenDeployments(dashboard);
                FailedDeployments(dashboard);
                CompletedDeployments(dashboard);

                if (dashboard.Items.All(i => i.IsCurrent && !i.HasWarningsOrErrors))
                {
                    OnErrorsCleared();
                }
                else if (dashboard.Items.Any(i => i.IsCurrent && i.HasWarningsOrErrors))
                {
                    OnErrorsFound();
                }

                _lastElapsed = now;
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
            finally
            {
                if (Debugger.IsAttached)
                {
                    _pollingTimer.Start();
                }
            }
        }

        private void FixedDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in dashboard.PreviousItems
                        where i.ReleaseId == p.ReleaseId &&
                              i.EnvironmentId == p.EnvironmentId &&
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
                        from p in dashboard.PreviousItems
                        where i.ReleaseId == p.ReleaseId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              !i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            items = items.Union(dashboard.Items.Where(i => !i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !dashboard.PreviousItems.Any(p => i.ReleaseId == p.ReleaseId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard)));

            if (items.Any())
            {
                OnDeploymentSucceeded(new DeploymentEventArgs
                {
                    Items = items.ToList()
                });
            }
        }

        private void NewBrokenDeployments(DashboardResource dashboard)
        {
            var items = from i in dashboard.Items
                        from p in dashboard.PreviousItems
                        where i.ReleaseId == p.ReleaseId &&
                              i.EnvironmentId == p.EnvironmentId &&
                              i.HasWarningsOrErrors && !p.HasWarningsOrErrors &&
                              i.CompletedTime >= _lastElapsed
                        select i.ToDeploymentResult(dashboard);

            items = items.Union(dashboard.Items.Where(i => i.HasWarningsOrErrors && i.CompletedTime >= _lastElapsed && !dashboard.PreviousItems.Any(p => i.ReleaseId == p.ReleaseId && i.EnvironmentId == p.EnvironmentId)).Select(i => i.ToDeploymentResult(dashboard)));

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
                        from p in dashboard.PreviousItems
                        where i.ReleaseId == p.ReleaseId &&
                              i.EnvironmentId == p.EnvironmentId &&
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
