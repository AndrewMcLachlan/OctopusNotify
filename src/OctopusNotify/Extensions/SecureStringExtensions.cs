using System.Runtime.InteropServices;

namespace System.Security
{
    public static class SecureStringExtensions
    {
        public static string ToUnsecureString(this SecureString str)
        {
            if (str == null) return null;
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
