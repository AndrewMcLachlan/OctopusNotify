using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OctopusNotify.App.Models;

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NotifyIconWindow : Window
    {
        #region Fields
        private Queue<Notification> _notificationQueue = new Queue<Notification>();
        private static bool BalloonVisible;
        private static object SyncRoot = new object();
        #endregion

        #region Constructors
        public NotifyIconWindow()
        {
            Visibility = Visibility.Hidden;
            InitializeComponent();

            ((NotifyIconViewModel)DataContext).Notification += NotifyIconWindow_Notification;
        }

        #endregion

        #region Event Handlers

        private void NotifyIconWindow_Notification(object sender, NotificationEventArgs e)
        {
            if (!e.Notifications.Any()) return;

            lock (SyncRoot)
            {
                if (BalloonVisible)
                {
                    foreach (var n in e.Notifications)
                    {
                        _notificationQueue.Enqueue(n);
                    }
                }
                else
                {
                    DispatcherHelper.Run(ShowBalloon, e.Notifications.First());
                    foreach (var n in e.Notifications.Skip(1))
                    {
                        _notificationQueue.Enqueue(n);
                    }
                }
            }
        }

        private void NotifyIcon_BalloonShowing(object sender, RoutedEventArgs e)
        {
            lock (SyncRoot)
            {
                BalloonVisible = true;
            }
        }

        private void NotifyIcon_BalloonClosing(object sender, RoutedEventArgs e)
        {
            lock (SyncRoot)
            {
                if (_notificationQueue.Any())
                {
                    Notification notification = _notificationQueue.Dequeue();
                    DispatcherHelper.Run(ShowBalloon, notification);
                }
                else
                {
                    BalloonVisible = false;
                }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
        }
        #endregion

        #region Private Methods

        private void ShowBalloon(Notification notification)
        {
            Balloon balloon = new Balloon(notification);

            Hardcodet.Wpf.TaskbarNotification.TaskbarIcon.AddBalloonShowingHandler(balloon, NotifyIcon_BalloonShowing);
            Hardcodet.Wpf.TaskbarNotification.TaskbarIcon.AddBalloonClosingHandler(balloon, NotifyIcon_BalloonClosing);

            NotifyIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, 10000);
        }
        #endregion
    }
}
