using System;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Exceptions;
using Octopus.Client.Model;

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
        public async Task<(bool, string)> Test(Uri url, string apiKey)
        {
            if (url == null) return (false, "No URL!");

            try
            {
                Task<ServerStatusResource> task = Task.Factory.StartNew(() =>
                {
                    OctopusServerEndpoint endpoint = new OctopusServerEndpoint(url.ToString(), apiKey);
                    OctopusRepository repo = new OctopusRepository(endpoint);
                    return repo.ServerStatus.GetServerStatus();
                });
                if (await Task.WhenAny(task, Task.Delay(5000)) == task)
                {
                    if (!task.IsFaulted)
                    {
                        return (true, "Connection succeeded!");
                    }
                    else if (task.Exception != null)
                    {
                        return (false, GetMessageFromException(task.Exception.InnerException));
                    }

                    return (false, "Unknown connection error.");
                }

                return (false, "Connection timed out.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private string GetMessageFromException(Exception exception)
        {
            if (exception == null) return String.Empty;

            if (exception is OctopusSecurityException)
            {
                if (((OctopusSecurityException)exception).HttpStatusCode == 401)
                {
                    return "Invalid API Key or credentials";
                }
            }

            if (exception is UnsupportedApiVersionException)
            {
                return "Octopus Notify does not support this version of Octopus Server";
            }

            return $"Octopus Server reports: {exception.Message}";
        }
    }
}
