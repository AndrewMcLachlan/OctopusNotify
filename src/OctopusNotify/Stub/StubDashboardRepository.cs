using System;
using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusNotify.Stub
{
    public class StubDashboardRepository : IDashboardRepository
    {
        private static bool _buildOnePreviousBroken;
        private static bool _buildOneBroken;
        private static bool _buildTwoPreviousBroken;
        private static bool _buildTwoBroken;
        private static bool _buildThreePreviousBroken;
        private static bool _buildThreeBroken;

        public static int deploymentCounter = 1;

        public static bool BuildOneBroken
        {
            get
            {
                return _buildOneBroken;
            }
            set
            {
                _buildOnePreviousBroken = _buildOneBroken;
                _buildOneBroken = value;
                BuildOneLastUpdate = DateTime.Now;
            }
        }


        public static bool BuildTwoBroken
        {
            get
            {
                return _buildTwoBroken;
            }
            set
            {
                _buildTwoPreviousBroken = _buildTwoBroken;
                _buildTwoBroken = value;
                BuildTwoLastUpdate = DateTime.Now;
            }
        }


        public static bool BuildThreeBroken
        {
            get
            {
                return _buildThreeBroken;
            }
            set
            {
                _buildThreePreviousBroken = _buildThreeBroken;
                _buildThreeBroken = value;
                BuildThreeLastUpdate = DateTime.Now;
            }
        }

        public static DateTime BuildOneLastUpdate = DateTime.MinValue.AddHours(12);
        public static DateTime BuildTwoLastUpdate = DateTime.MinValue.AddHours(12);
        public static DateTime BuildThreeLastUpdate = DateTime.MinValue.AddHours(12);

        public DashboardResource GetDashboard()
        {
            DashboardItemResource build1 = GetBuild("1", BuildOneBroken, BuildOneLastUpdate);
            DashboardItemResource build2 = GetBuild("2", BuildTwoBroken, BuildTwoLastUpdate);
            DashboardItemResource build3 = GetBuild("3", BuildThreeBroken, BuildThreeLastUpdate);

            DashboardItemResource build1Previous = GetBuild("1", _buildOnePreviousBroken, BuildOneLastUpdate.AddSeconds(-30));
            DashboardItemResource build2Previous = GetBuild("2", _buildTwoPreviousBroken, BuildTwoLastUpdate.AddSeconds(-30));
            DashboardItemResource build3Previous = GetBuild("3", _buildThreePreviousBroken, BuildThreeLastUpdate.AddSeconds(-30));

            /*Random random = new Random(Guid.NewGuid().GetHashCode());
            int rand = random.Next(0, 3);

            DashboardItemResource di;
            if (rand == 0)
            {
                di = new DashboardItemResource
                {
                    CompletedTime = DateTime.Now,
                    IsCompleted = true,
                    IsCurrent = true,
                    HasWarningsOrErrors = true,
                    ErrorMessage = "AN ERROR HAS OCURRED!",
                    ProjectId ="1",
                    EnvironmentId = "1",
                    DeploymentId = "1",
                    ReleaseVersion = "6.2.30.0",
                    ReleaseId = "1",
                };
            }
            else
            {
                di = new DashboardItemResource
                {
                    CompletedTime = DateTime.Now,
                    IsCompleted = true,
                    IsCurrent = true,
                    HasWarningsOrErrors = false,
                    ProjectId = "1",
                    EnvironmentId = "1",
                    DeploymentId = "1",
                    ReleaseVersion = "6.2.30.0",
                    ReleaseId = "1",
                };
            }*/

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
                },
                PreviousItems = new List<DashboardItemResource>
                {
                    build1Previous,
                    build2Previous,
                    build3Previous,
                }
            };
        }

        private DashboardItemResource GetBuild(string projectId, bool buildBroken, DateTime lastUpdate)
        {
            DashboardItemResource di;
            if (buildBroken)
            {
                di = new DashboardItemResource
                {
                    CompletedTime = lastUpdate,
                    IsCompleted = true,
                    IsCurrent = true,
                    HasWarningsOrErrors = true,
                    ErrorMessage = "AN ERROR HAS OCURRED!",
                    ProjectId = projectId,
                    EnvironmentId = "1",
                    DeploymentId = "1",
                    ReleaseVersion = "6.2.30." + deploymentCounter.ToString(),
                    ReleaseId = projectId,
                };
            }
            else
            {
                di = new DashboardItemResource
                {
                    CompletedTime = lastUpdate,
                    IsCompleted = true,
                    IsCurrent = true,
                    HasWarningsOrErrors = false,
                    ProjectId = projectId,
                    EnvironmentId = "1",
                    DeploymentId = deploymentCounter++.ToString(),
                    ReleaseVersion = "6.2.30." + deploymentCounter.ToString(),
                    ReleaseId = projectId,
                };
            }

            return di;
        }

        public DashboardResource GetDynamicDashboard(string[] projects, string[] environments)
        {
            throw new NotImplementedException();
        }
    }
}
