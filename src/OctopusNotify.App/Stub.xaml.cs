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
using OctopusNotify.Stub;

namespace OctopusNotify.App
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
            StubDashboardRepository.BuildOneBroken = true;
            StubDashboardRepository.BuildOneLastUpdate = DateTime.Now;
            Build1Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Break2_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildTwoBroken = true;
            Build2Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Break3_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildThreeBroken = true;
            Build3Status.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Success1_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildOneBroken = false;
            Build1Status.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Success2_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildTwoBroken = false;
            Build2Status.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Success3_Click(object sender, RoutedEventArgs e)
        {
            StubDashboardRepository.BuildThreeBroken = false;
            Build3Status.Fill = new SolidColorBrush(Colors.Green);
        }
    }
}
