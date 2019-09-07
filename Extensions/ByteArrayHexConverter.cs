using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ByteArrayHexConverter
    {
        public static byte[] ToByteArray(this String hexInput)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return bytes;
        }
        public static String ToHex(this byte[] input)
        {
            StringBuilder sbBytes = new StringBuilder(input.Length * 2);
            foreach (byte b in input)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
    }
}
