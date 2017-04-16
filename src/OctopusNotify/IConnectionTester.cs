using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace OctopusNotify
{
    public interface IConnectionTester
    {
        Task<(bool, string)> Test(Uri url, string apiKey);

        Task<(bool, string)> Test();

        Task<(bool, string)> Test(Uri url, string userName, SecureString password);
    }
}
