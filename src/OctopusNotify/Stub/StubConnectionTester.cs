using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace OctopusNotify.Stub
{
    public class StubConnectionTester : IConnectionTester
    {
        public Task<(bool, string)> Test(Uri url, string apiKey)
        {
            return Task.Factory.StartNew(() => (true, "Connection succeeded"));
        }

        public Task<(bool, string)> Test()
        {
            return Task.Factory.StartNew(() => (true, "Connection succeeded"));
        }

        public Task<(bool, string)> Test(Uri url, string userName, SecureString password)
        {
            throw new NotImplementedException();
        }
    }
}
