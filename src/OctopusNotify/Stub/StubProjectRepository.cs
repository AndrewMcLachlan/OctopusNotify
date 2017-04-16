using System;
using System.Collections.Generic;
using System.IO;
using Octopus.Client.Editors;
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

        public ProjectEditor CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle)
        {
            throw new NotImplementedException();
        }

        public ProjectEditor CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle, string description)
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

        public List<ProjectResource> FindAll(string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindByName(string name, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindByNames(IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindByNames(IEnumerable<string> names, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindMany(Func<ProjectResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> FindMany(Func<ProjectResource, bool> search, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindOne(Func<ProjectResource, bool> search)
        {
            throw new NotImplementedException();
        }

        public ProjectResource FindOne(Func<ProjectResource, bool> search, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public ProjectResource Get(string idOrHref)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> Get(params string[] ids)
        {
            throw new NotImplementedException();
        }

        public List<ProjectResource> GetAll()
        {
            return new List<ProjectResource>
            {
                new ProjectResource { Id = "1", Name = "Asm.WebServices.ReferenceDataService.Dep" },
                new ProjectResource { Id = "2", Name = "Admin" },
                new ProjectResource { Id = "3", Name = "Business" },
            };
        }

        public IReadOnlyList<ReleaseResource> GetAllReleases(ProjectResource project)
        {
            throw new NotImplementedException();
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

        public ResourceCollection<ProjectTriggerResource> GetTriggers(ProjectResource project)
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

        public void Paginate(Func<ResourceCollection<ProjectResource>, bool> getNextPage, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public ProjectResource Refresh(ProjectResource resource)
        {
            throw new NotImplementedException();
        }

        public void SetLogo(ProjectResource project, string fileName, Stream contents)
        {
            throw new NotImplementedException();
        }
    }
}
