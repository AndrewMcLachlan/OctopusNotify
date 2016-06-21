using System;
using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusNotify.Stub
{
    public class StubProjectRepository : IProjectRepository
    {
        public ProjectResource Create(ProjectResource resource)
        {
            throw new NotImplementedException();
        }

        public void Delete(ProjectResource resource)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindAll()
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindByNames(IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindMany(Func<ProjectResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindOne(Func<ProjectResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public ProjectResource Get(string idOrHref)
        {
            throw new NotImplementedException();
        }

        public List<ReferenceDataItem> GetAll()
        {
            return new List<ReferenceDataItem>
            {
                new ReferenceDataItem("1", "Asm.WebServices.ReferenceDataService.Dep"),
                new ReferenceDataItem("2", "Admin"),
                new ReferenceDataItem("3", "Business"),
            };
        }

        public ResourceCollection<ChannelResource> GetChannels(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public ReleaseResource GetReleaseByVersion(ProjectResource project, string version)
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<ReleaseResource> GetReleases(ProjectResource project, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public ProjectResource Modify(ProjectResource resource)
        {
            throw new NotImplementedException();
        }

        public void Paginate(Func<ResourceCollection<ProjectResource>, bool> getNextPage)
        {
            throw new NotImplementedException();
        }

        public ProjectResource Refresh(ProjectResource resource)
        {
            throw new NotImplementedException();
        }
    }
}
