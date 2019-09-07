using Microsoft.Extensions.Caching.Memory;
using System;

namespace NetCore.Captcha
{
    public static class Store
    {
        public static int ExpirationMinutes = 10;
        private static IMemoryCache captchas = new MemoryCache(new MemoryCacheOptions());
        public static void MakeUsed(long CaptchaID) {
            captchas.Set(CaptchaID, true, DateTimeOffset.Now.AddMinutes(ExpirationMinutes));
        }

        internal static object IsUsed(long CaptchaID)
        {
            return captchas.Get(CaptchaID);
        }
    }
}
