//**************************************
// Copied from http://stackoverflow.com/questions/952080/how-do-you-select-the-right-size-icon-from-a-multi-resolution-ico-file-in-wpf
//**************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace OctopusNotify.App.MarkupExtensions
{
    public class IconExtension : MarkupExtension
    {
        private string _source;

        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                // Have to make full pack URI from short form, so System.Uri recognizes it.
                _source = "pack://application:,,,/OctopusNotify;component" + value;
            }
        }

        public int Size { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var decoder = BitmapDecoder.Create(new Uri(Source),
                                               BitmapCreateOptions.DelayCreation,
                                               BitmapCacheOption.OnDemand);

            var result = decoder.Frames.SingleOrDefault(f => f.Width == Size);
            if (result == default(BitmapFrame))
            {
                result = decoder.Frames.OrderBy(f => f.Width).First();
            }

            return result;
        }
    }
}
