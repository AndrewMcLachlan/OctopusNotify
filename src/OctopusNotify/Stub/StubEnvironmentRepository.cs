using System;
using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusNotify.Stub
{
    public class StubEnvironmentRepository : IEnvironmentRepository
    {
        public EnvironmentResource Create(EnvironmentResource resource)
        {
            throw new NotImplementedException();
        }

        public void Delete(EnvironmentResource resource)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindAll()
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindByNames(IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindMany(Func<EnvironmentResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource FindOne(Func<EnvironmentResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource Get(string idOrHref)
        {
            throw new NotImplementedException();
        }

        public List<ReferenceDataItem> GetAll()
        {
            return new List<ReferenceDataItem>
            {
                new ReferenceDataItem("1", "AsmProj-ST"),
                new ReferenceDataItem("2", "AsmProj-SIT"),
                new ReferenceDataItem("3", "AsmProj-UAT"),
            };
        }

        public List<MachineResource> GetMachines(EnvironmentResource environment)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource Modify(EnvironmentResource resource)
        {
            throw new NotImplementedException();
        }

        public void Paginate(Func<ResourceCollection<EnvironmentResource>, bool> getNextPage)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource Refresh(EnvironmentResource resource)
        {
            throw new NotImplementedException();
        }

        public void Sort(string[] environmentIdsInOrder)
        {
            throw new NotImplementedException();
        }
    }
}
