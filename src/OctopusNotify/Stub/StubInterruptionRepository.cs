using System;
using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusNotify.Stub
{
    internal class StubInterruptionRepository : IInterruptionRepository
    {
        public static DateTime BuildOneLastUpdate { get; internal set; }
        public static DateTime BuildThreeLastUpdate { get; internal set; }
        public static DateTime BuildTwoLastUpdate { get; internal set; }

        public InterruptionResource Get(string idOrHref)
        {
            return new InterruptionResource
            {
                Id = idOrHref,
                IsPending = true,
                Created = DateTime.Now,
            };
        }

        public UserResource GetResponsibleUser(InterruptionResource interruption)
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<InterruptionResource> List(int skip = 0, bool pendingOnly = false, string regardingDocumentId = null)
        {
            return new ResourceCollection<InterruptionResource>(new List<InterruptionResource>
            {
                new InterruptionResource
                {
                    TaskId = regardingDocumentId,
                    Id = regardingDocumentId,
                    IsPending = true,
                    Created = regardingDocumentId == "1" ? BuildOneLastUpdate :
                              regardingDocumentId == "2" ? BuildTwoLastUpdate :
                              BuildThreeLastUpdate,
                }
            },
            new LinkCollection());
        }

        public InterruptionResource Refresh(InterruptionResource resource)
        {
            throw new NotImplementedException();
        }

        public void Submit(InterruptionResource interruption)
        {
            throw new NotImplementedException();
        }

        public void TakeResponsibility(InterruptionResource interruption)
        {
            throw new NotImplementedException();
        }
    }
}