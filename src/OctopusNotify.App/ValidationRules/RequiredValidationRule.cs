using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OctopusNotify.App.ValidationRules
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string emptyCheck = value as string;
            if (value == null || (emptyCheck != null && String.IsNullOrWhiteSpace(emptyCheck)))
            {
                return new ValidationResult(false, "Please provide a value");
            }

            return new ValidationResult(true, null);
        }
    }
}
