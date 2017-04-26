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

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for Signin.xaml
    /// </summary>
    public partial class Signin : Window
    {
        public Signin()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var _adapter = Container.Current.Resolve<IDeploymentRepositoryAdapter>();
            if (_adapter.SignIn(UserName.Text, Password.Password))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("User name or password incorrect", "Octopus Notify", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
