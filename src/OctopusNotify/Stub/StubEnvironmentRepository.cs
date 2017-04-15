using System;
using System.Collections.Generic;
using Octopus.Client.Editors;
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

        public EnvironmentEditor CreateOrModify(string name)
        {
            throw new NotImplementedException();
        }

        public EnvironmentEditor CreateOrModify(string name, string description)
        {
            throw new NotImplementedException();
        }

        public void Delete(EnvironmentResource resource)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindAll(string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource FindByName(string name, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindByNames(IEnumerable<string> names, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> FindMany(Func<EnvironmentResource, bool> search, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource FindOne(Func<EnvironmentResource, bool> search, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public EnvironmentResource Get(string idOrHref)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> Get(params string[] ids)
        {
            throw new NotImplementedException();
        }

        public List<EnvironmentResource> GetAll()
        {
            return new List<EnvironmentResource>
            {
                new EnvironmentResource { Id = "1", Name = "AsmProj-ST" },
                new EnvironmentResource { Id = "2", Name ="AsmProj-SIT" },
                new EnvironmentResource { Id = "3", Name = "AsmProj-UAT" },
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

        public void Paginate(Func<ResourceCollection<EnvironmentResource>, bool> getNextPage, string path = null, object pathParameters = null)
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
