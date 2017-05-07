using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Models;
using OctopusNotify.App.Properties;

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for Signin.xaml
    /// </summary>
    public partial class Signin : Window
    {
        public static DependencyProperty TestModeProperty = DependencyProperty.Register("TestMode", typeof(bool), typeof(Signin), new FrameworkPropertyMetadata());

        public bool TestMode
        {
            get { return (bool)GetValue(TestModeProperty); }
            set { SetValue(TestModeProperty, value); }
        }

        public Signin() : this(false)
        {
        }

        public Signin(bool testMode)
        {
            InitializeComponent();
            TestMode = testMode;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var adapter = Container.Current.Resolve<IDeploymentRepositoryAdapter>();
            if (adapter.SignIn(UserName.Text, Password.SecurePassword))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("User name or password incorrect", "Octopus Notify", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
            /*var tester = Container.Current.Resolve<IConnectionTester>();

            Uri server = null;
            Uri.TryCreate(Settings.Default.ServerUrl, UriKind.Absolute, out server);

            tester.Test(server, UserName.Text, Password.SecurePassword);*/
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
