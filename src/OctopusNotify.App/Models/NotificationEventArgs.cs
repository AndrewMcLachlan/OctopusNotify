using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusNotify.App.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public IReadOnlyList<Notification> Notifications { get; private set; }

        public NotificationEventArgs(IEnumerable<Notification> notifications)
        {
            Notifications = notifications.ToList();
        }
    }
}
