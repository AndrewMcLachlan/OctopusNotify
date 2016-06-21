using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Interop.Imaging;

namespace System.Drawing
{
    public static class IconExtensions
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        private static Dictionary<int, ImageSource> Cache = new Dictionary<int, ImageSource>();

        public static ImageSource ToImageSource(this Icon icon)
        {
            int hashCode = icon.GetHashCode();
            if (Cache.ContainsKey(hashCode))
            {
                return Cache[hashCode];
            }

            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            var imageSource = CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            Cache.Add(hashCode, imageSource);

            return imageSource;
        }
    }
}
