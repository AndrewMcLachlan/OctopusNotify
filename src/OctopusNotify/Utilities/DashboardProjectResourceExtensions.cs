using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.Utilities
{
    public static class DashboardProjectResourceExtensions
    {
        public static Project ToProject(this DashboardProjectResource project)
        {
            return new Project
            {
                Name = project.Name,
                ProjectId = project.Id,
            };
        }
    }
}
