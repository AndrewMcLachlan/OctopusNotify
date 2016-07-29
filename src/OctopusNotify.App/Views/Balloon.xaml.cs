using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;
using OctopusNotify.App.Models;

namespace OctopusNotify.App.Views
{
    public partial class Balloon : UserControl
    {
        public static DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty VersionProperty = DependencyProperty.Register("Version", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty EnvironmentProperty = DependencyProperty.Register("Environment", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty LinkProperty = DependencyProperty.Register("Link", typeof(Uri), typeof(Balloon), new FrameworkPropertyMetadata());

        private static readonly Dictionary<Models.BalloonIcon, ImageSource> BalloonIcons = new Dictionary<Models.BalloonIcon, ImageSource>
        {
            { Models.BalloonIcon.Success, new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/Images/Green Tick.png")) },
            { Models.BalloonIcon.Error, SystemIcons.Error.ToImageSource() },
            { Models.BalloonIcon.Warning, SystemIcons.Warning.ToImageSource() },
            { Models.BalloonIcon.Question, SystemIcons.Question.ToImageSource() },
        };

        public string Project
        {
            get { return GetValue(ProjectProperty) as string; }
            set { SetValue(ProjectProperty, value);  }
        }

        public string Version
        {
            get { return GetValue(VersionProperty) as string; }
            set { SetValue(VersionProperty, value); }
        }

        public string Environment
        {
            get { return GetValue(EnvironmentProperty) as string; }
            set { SetValue(EnvironmentProperty, value); }
        }

        public Uri Link
        {
            get { return GetValue(LinkProperty) as Uri; }
            set {SetValue(LinkProperty, value); }
        }

        public Balloon(string title, string project, string version, string environment, Uri link, Icon icon) : this(title, project, version, environment, link, icon.ToImageSource())
        {
        }

        public Balloon(string title, string project, string version, string environment, Uri link, ImageSource icon)
        {
            InitializeComponent();

            Title.Text = title;
            Project = project;
            Version = version;
            Environment = environment;
            Link = link;

            Icon.Source = icon;
        }

        public Balloon(Notification notification) : this(notification.Message, notification.ProjectName, notification.Version, notification.EnvironmentName, notification.Link, BalloonIcons[notification.Icon])
        {
        }

        private void Close_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
            e.Handled = true;
        }

        private void BalloonControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Link != null)
            {
                System.Diagnostics.Process.Start(Link.ToString());
            }
        }
    }
}
