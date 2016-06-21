using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client;

namespace OctopusNotify
{
    public class ConnectionTester
    {
        public async Task<bool> Test(Uri url, string apiKey)
        {
            if (url == null) return false;

            OctopusServerEndpoint endpoint = new OctopusServerEndpoint(url.ToString(), apiKey);
            
            OctopusRepository repo = new OctopusRepository(endpoint);
            try
            {
                Task task = Task.Factory.StartNew(() => repo.Dashboards.GetDashboard());
                if (await Task.WhenAny(task, Task.Delay(5000)) == task)
                {
                    return !task.IsFaulted;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
