using System;
using System.Collections.Generic;
using Octopus.Client.Model;

namespace OctopusNotify.Utilities
{
    public class DashboardItemResourceComparer : IEqualityComparer<DashboardItemResource>
    {
        public bool Equals(DashboardItemResource x, DashboardItemResource y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.ProjectId == y.ProjectId &&
                   x.EnvironmentId == y.EnvironmentId;
        }

        public int GetHashCode(DashboardItemResource obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.ProjectId.GetHashCode() ^ obj.EnvironmentId.GetHashCode();
        }
    }
}
