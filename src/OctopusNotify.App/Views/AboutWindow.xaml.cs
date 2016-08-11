using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        #region Constructors
        public AboutWindow()
        {
            InitializeComponent();

            Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

            string formatText;

            if (version.Build > 0)
            {
                formatText = "Version {0}.{1}.{2}";
            }
            else
            {
                formatText = "Version {0}.{1}";
            }

            Version.Text = String.Format(formatText, version.Major, version.Minor, version.Build, version.Revision);
            Opacity = 0;
        }
        #endregion

        #region Event Handlers
        private void About_Deactivated(object sender, EventArgs e)
        {
            AnimateClose();
        }

        private void About_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnimateClose();
        }


        private void About_Activated(object sender, EventArgs e)
        {
            AnimateOpen();
        }
        #endregion

        #region Private Methods
        private void AnimateOpen()
        {
            var anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(100));
            BeginAnimation(OpacityProperty, anim);
        }

        private void AnimateClose()
        {
            var anim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(100));
            anim.Completed += (s, ev) => Close();
            BeginAnimation(OpacityProperty, anim);
        }
        #endregion
    }
}
