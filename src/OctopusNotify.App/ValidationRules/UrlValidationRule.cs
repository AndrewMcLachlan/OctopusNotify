using System;
using System.Globalization;
using System.Windows.Controls;

namespace OctopusNotify.App.ValidationRules
{
    public class UrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is Uri && value != null)
            {
                return new ValidationResult(true, null);
            }

            string url = value as string;
            Uri uri;
            if (url == null || !Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return new ValidationResult(false, "Please provide a valid URL");
            }

            return new ValidationResult(true, null);
        }
    }
}
