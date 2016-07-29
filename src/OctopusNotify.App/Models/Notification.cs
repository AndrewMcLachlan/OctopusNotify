using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OctopusNotify.App.Models
{
    public class Notification
    {
        public BalloonIcon Icon { get; set; }

        public string Message { get; set; }

        public string ProjectName { get; set; }

        public string EnvironmentName { get; set; }

        public string Version { get; set; }

        public Uri Link { get; set; }
    }
}
