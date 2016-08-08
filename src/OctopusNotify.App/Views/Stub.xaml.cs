using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Octopus.Client.Model;
using OctopusNotify.Model;
using OctopusNotify.Stub;

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for Stub.xaml
    /// </summary>
    public partial class Stub : Window
    {
        public Stub()
        {
            InitializeComponent();
        }

        private void Break1_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildOneLastUpdate = DateTime.Now;
            Build1Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Break2_Click(object sender, RoutedEventArgs e)
        {
            Build2Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Break3_Click(object sender, RoutedEventArgs e)
        {
            Build3Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Success1_Click(object sender, RoutedEventArgs e)
        {
            Build1Status.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Success2_Click(object sender, RoutedEventArgs e)
        {
            Build2Status.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Success3_Click(object sender, RoutedEventArgs e)
        {
            Build3Status.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Replace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Replace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string parameter = e.Parameter as string;

            string[] split = parameter.Split('|');

            string status = split[1];

            string buildNum = split[0];

            DeploymentStatus state = (DeploymentStatus)Enum.Parse(typeof(DeploymentStatus), status);

            if (buildNum == "1")
            {
                StubDashboardRepository.BuildOneStatus = state;
            }
            if (buildNum == "2")
            {
                StubDashboardRepository.BuildTwoStatus = state;
            }
            if (buildNum == "3")
            {
                StubDashboardRepository.BuildThreeStatus = state;
            }

            e.Handled = true;

        }
    }
}
