using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OctopusNotify.App.Utilities
{
    internal static class EncryptionHelper
    {
        private static readonly byte[] entropy = Encoding.Unicode.GetBytes("Just making this a little bit harder to crack.");

        public static string Encrypt(this string str)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(str), entropy, DataProtectionScope.CurrentUser));
        }

        public static string Decrypt(this string str)
        {
            if (String.IsNullOrWhiteSpace(str)) return str;
            return Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(str), entropy, DataProtectionScope.CurrentUser));
        }
    }
}
