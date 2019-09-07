using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha
{
    static class Utils
    {
        private static readonly byte[] entropyBytes = Encoding.ASCII.GetBytes("01");
        private static readonly System.Security.Cryptography.DataProtectionScope scope = System.Security.Cryptography.DataProtectionScope.LocalMachine;

        public static string Crypt(this string text)
        {
            byte[] Protected = System.Security.Cryptography.ProtectedData.Protect(Encoding.ASCII.GetBytes(text), entropyBytes, scope);
            string base64 = Convert.ToBase64String(Protected);
            string hex = Protected.ToHex();
            return hex;
        }

        public static string Decrypt(this string text)
        {
            
            return Encoding.ASCII.GetString(System.Security.Cryptography.ProtectedData.Unprotect(text.ToByteArray(), entropyBytes, scope));
        }
    }
}
