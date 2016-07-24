using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace OctopusNotify.App
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Balloon : UserControl
    {
        public static DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty VersionProperty = DependencyProperty.Register("Version", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty EnvironmentProperty = DependencyProperty.Register("Environment", typeof(string), typeof(Balloon), new FrameworkPropertyMetadata());
        public static DependencyProperty LinkProperty = DependencyProperty.Register("Link", typeof(Uri), typeof(Balloon), new FrameworkPropertyMetadata());

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
