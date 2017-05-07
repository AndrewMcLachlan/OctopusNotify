using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.App.ViewModels;

namespace OctopusNotify.App.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow2.xaml
    /// </summary>
    public partial class SettingsWindow2 : Window
    {
        private static Regex PositiveInteger = new Regex("[0-9]+");
        private static Regex Zero = new Regex("^0$");

        private int _validationErrorCount = 0;
        private bool _apiKeyChanged = false;
        private bool _visualTextChange = false;

        public SettingsWindow2()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((SettingsViewModel)DataContext).IsValid;
            e.Handled = true;
        }

        private void Test_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((SettingsViewModel)DataContext).CanTest;
            e.Handled = true;
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dc = (SettingsViewModel)DataContext;

            if (_apiKeyChanged)
            {
                dc.Save(ApiKeyText.Text);
            }
            else
            {
                dc.Save();
            }

            e.Handled = true;
            Close();
        }

        private void TestCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dc = ((SettingsViewModel)DataContext);

            if (!dc.CanTest) return;

            if (LoginWithPassword.IsChecked ?? false)
            {
                Signin signinWindow = new Signin(true);

                if (signinWindow.ShowDialog() ?? false)
                {
                    dc.Test(signinWindow.UserName.Text, signinWindow.Password.SecurePassword).ContinueWith(t =>
                    {
                        if (t.Result.Item1)
                        {
                            DispatcherHelper.Run(Dispatcher, () => MessageBox.Show(this, t.Result.Item2, "Test", MessageBoxButton.OK, MessageBoxImage.Information));
                        }
                        else
                        {
                            DispatcherHelper.Run(Dispatcher, () => MessageBox.Show(this, t.Result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Warning));
                        }
                    });
                }
            }
            else
            {
                dc.Test(_apiKeyChanged ? ApiKeyText.Text : Settings.Default.ApiKey.Decrypt()).ContinueWith(t =>
                {
                    if (t.Result.Item1)
                    {
                        DispatcherHelper.Run(Dispatcher, () => MessageBox.Show(this, t.Result.Item2, "Test", MessageBoxButton.OK, MessageBoxImage.Information));
                    }
                    else
                    {
                        DispatcherHelper.Run(Dispatcher, () => MessageBox.Show(this, t.Result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Warning));
                    }
                });
            }
            e.Handled = true;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            _validationErrorCount = e.Action == ValidationErrorEventAction.Added ? _validationErrorCount + 1 : _validationErrorCount - 1;
            ((SettingsViewModel)DataContext).IsValid = _validationErrorCount <= 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //Hide();
        }

        private void ApiKeyText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (ViewModel != null)
            //{
            ((SettingsViewModel)DataContext).ApiKeyChanged = !_visualTextChange;
            //}
            _visualTextChange = false;
        }

        private void Time_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidateTime(e.Text);
        }

        private bool ValidateTime(string text)
        {
            return PositiveInteger.IsMatch(text);
        }

        private void Time_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!ValidateTime(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ApiKeyText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ApiKeyText.Text == ((SettingsViewModel)DataContext).InitialApiKey)
            {
                _visualTextChange = true;
                ApiKeyText.Text = String.Empty;
                e.Handled = true;
            }

        }

        private void ApiKeyText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ApiKeyText.Text) && (!((SettingsViewModel)DataContext)?.ApiKeyChanged ?? false))
            {
                _visualTextChange = true;
                ApiKeyText.Text = ((SettingsViewModel)DataContext).InitialApiKey;
                e.Handled = true;
            }

        }

        private void ApiKeyText_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
