using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace NetCore.Captcha
{
    public static class CaptchaGenerator
    {
        public static string ToBase64(this Captcha captcha)
        {
            byte[] buffer = captcha.ToStream().GetBuffer();
            return new StringBuilder("data:")
                    .Append(System.Net.Mime.MediaTypeNames.Image.Jpeg)
                    .Append(";base64,")
                    .Append(Convert.ToBase64String(buffer))
                    .ToString();
        }

    }
}
