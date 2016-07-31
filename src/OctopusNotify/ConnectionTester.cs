using System;
using System.Threading.Tasks;
using Octopus.Client;

namespace OctopusNotify
{
    /// <summary>
    /// Tests a connection.
    /// </summary>
    /// <remarks>
    /// Currently this does not use IoC.
    /// </remarks>
    public class ConnectionTester
    {
        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <param name="url">The connection URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>true if the connection is successful, otherwise false.</returns>
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
