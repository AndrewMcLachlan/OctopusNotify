using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusNotify.App.ViewModels
{
    public class AboutViewModel
    {
        public Uri WpfIconUri
        {
            get
            {
                return String.IsNullOrEmpty(ConfigurationManager.AppSettings["ack:WpfIcon"]) ?
                    new Uri("http://www.hardcodet.net/wpf-notifyicon") :
                    new Uri(ConfigurationManager.AppSettings["ack:WpfIcon"]);
            }
        }

        public string WpfIconLinkDisplay
        {
            get
            {
                return WpfIconUri.Host + WpfIconUri.PathAndQuery + WpfIconUri.Fragment;
            }
        }
    }
}
