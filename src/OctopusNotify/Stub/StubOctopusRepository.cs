using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Repositories;

namespace OctopusNotify.Stub
{
    public class StubOctopusRepository : IOctopusRepository
    {
        public IAccountRepository Accounts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IArtifactRepository Artifacts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IBackupRepository Backups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IBuiltInPackageRepositoryRepository BuiltInPackageRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICertificateRepository Certificates
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IChannelRepository Channels
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IOctopusClient Client
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDashboardConfigurationRepository DashboardConfigurations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDashboardRepository Dashboards
        {
            get
            {
                return new StubDashboardRepository();
            }
        }

        public IDefectsRepository Defects
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDeploymentProcessRepository DeploymentProcesses
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDeploymentRepository Deployments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnvironmentRepository Environments
        {
            get
            {
                return new StubEnvironmentRepository();
            }
        }

        public IEventRepository Events
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IFeedRepository Feeds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IInterruptionRepository Interruptions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ILibraryVariableSetRepository LibraryVariableSets
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ILifecyclesRepository Lifecycles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMachineRoleRepository MachineRoles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMachineRepository Machines
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IOctopusServerNodeRepository OctopusServerNodes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IProjectGroupRepository ProjectGroups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IProjectRepository Projects
        {
            get
            {
                return new StubProjectRepository();
            }
        }

        public IReleaseRepository Releases
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRetentionPolicyRepository RetentionPolicies
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IServerStatusRepository ServerStatus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITaskRepository Tasks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITeamsRepository Teams
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IUserRolesRepository UserRoles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IUserRepository Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IVariableSetRepository VariableSets
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
