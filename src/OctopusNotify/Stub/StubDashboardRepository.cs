using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using OctopusNotify.Model;

namespace OctopusNotify.Stub
{
    public class StubDashboardRepository : IDashboardRepository
    {
        private static readonly TaskState[] Completed = new[] { TaskState.Canceled, TaskState.Failed, TaskState.Success, TaskState.TimedOut };
        private static readonly TaskState[] Errors = new[] { TaskState.Failed, TaskState.TimedOut };

        private static DeploymentStatus _buildOneStatus = DeploymentStatus.Success;
        private static DeploymentStatus _buildTwoStatus = DeploymentStatus.Success;
        private static DeploymentStatus _buildThreeStatus = DeploymentStatus.Success;

        public static int deploymentCounter = 1;

        public static DeploymentStatus BuildOneStatus
        {
            get
            {
                return _buildOneStatus;
            }
            set
            {
                _buildOneStatus = value;
                BuildOneLastUpdate = DateTime.Now;
                StubInterruptionRepository.BuildOneLastUpdate = DateTime.Now;
            }
        }

        public static DeploymentStatus BuildTwoStatus
        {
            get
            {
                return _buildTwoStatus;
            }
            set
            {
                _buildTwoStatus = value;
                BuildTwoLastUpdate = DateTime.Now;
                StubInterruptionRepository.BuildTwoLastUpdate = DateTime.Now;
            }
        }

        public static DeploymentStatus BuildThreeStatus
        {
            get
            {
                return _buildThreeStatus;
            }
            set
            {
                _buildThreeStatus = value;
                BuildThreeLastUpdate = DateTime.Now;
                StubInterruptionRepository.BuildThreeLastUpdate = DateTime.Now;
            }
        }

        public static DateTime BuildOneLastUpdate = DateTime.MinValue.AddHours(12);
        public static DateTime BuildTwoLastUpdate = DateTime.MinValue.AddHours(12);
        public static DateTime BuildThreeLastUpdate = DateTime.MinValue.AddHours(12);

        public DashboardResource GetDashboard()
        {
            DashboardItemResource build1 = GetBuild("1", BuildOneStatus, BuildOneLastUpdate);
            DashboardItemResource build2 = GetBuild("2", BuildTwoStatus, BuildTwoLastUpdate);
            DashboardItemResource build3 = GetBuild("3", BuildThreeStatus, BuildThreeLastUpdate);

            //DashboardItemResource build4 = GetBuild("3", TaskState.Executing, DateTime.Now, true);
            //DashboardItemResource build5 = GetBuild("3", TaskState.Executing, DateTime.Now, true, true);

            return new DashboardResource
            {
                Projects = new List<DashboardProjectResource>
                {
                    new DashboardProjectResource { Id = "1", Name = "WebServices.ReferenceDataService.Dep" },
                    new DashboardProjectResource { Id = "2", Name = "WebServices.Admin.Dep" },
                    new DashboardProjectResource { Id = "3", Name = "Business" },
                },
                Environments = new List<DashboardEnvironmentResource>
                {
                    new DashboardEnvironmentResource { Id =  "1", Name = "AsmProj-ST" },
                    new DashboardEnvironmentResource{ Id = "2", Name = "AsmProj-SIT" },
                    new DashboardEnvironmentResource{ Id = "3", Name = "AsmProj-UAT" },
                },
                Items = new List<DashboardItemResource>
                {
                    build1,
                    build2,
                    build3,
                    //build4,
                   // build5,
                },
                PreviousItems = new List<DashboardItemResource>
                {
                }
            };
        }

        private DashboardItemResource GetBuild(string projectId, DeploymentStatus status, DateTime lastUpdate, bool hasPendingInterruptions = false, bool hasErrorOrWarning = false)
        {
            TaskState state;

            if (status == DeploymentStatus.GuidedFailure)
            {
                state = TaskState.Executing;
                hasErrorOrWarning = true;
                hasPendingInterruptions = true;
            }
            else if (status == DeploymentStatus.ManualStep)
            {
                state = TaskState.Executing;
                hasErrorOrWarning = false;
                hasPendingInterruptions = true;
            }
            else
            {
                state = (TaskState)(int)status;
            }

            bool isCompleted = Completed.Contains(state);
            bool error = Errors.Contains(state) || hasErrorOrWarning;

            return new DashboardItemResource
                {
                    CompletedTime = isCompleted ? lastUpdate : (DateTimeOffset?)null,
                    IsCompleted = isCompleted,
                    IsCurrent = true,
                    HasWarningsOrErrors = error,
                    HasPendingInterruptions = hasPendingInterruptions,
                    ErrorMessage = error ? "AN ERROR HAS OCURRED!" : null,
                    State = state,
                    ProjectId = projectId,
                    TaskId = projectId,
                    EnvironmentId = "1",
                    DeploymentId = deploymentCounter.ToString(),
                    ReleaseVersion = "1.2.30." + deploymentCounter.ToString(),
                    ReleaseId = projectId,
                };
        }

        public DashboardResource GetDynamicDashboard(string[] projects, string[] environments)
        {
            throw new NotImplementedException();
        }
    }
}
