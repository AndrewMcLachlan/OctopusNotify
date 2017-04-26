using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OctopusNotify.App.ValueConverters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility) && targetType != typeof(Visibility?))
            {
                throw new InvalidOperationException("The target must be System.Windows.Visibility");
            }

            bool input = (bool)value;

            if (input) return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
