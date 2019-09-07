using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha
{
    class OptionContainer
    {
        private static IDictionary<string, CaptchaOptions> options = new Dictionary<string, CaptchaOptions>();
        public static T GetOptions<T>() where T : CaptchaOptions
        {
            Type type = typeof(T);
            return (T)GetOptions(type);
        }
        public static CaptchaOptions GetOptions(string TypeOfExtendedCaptchaOptions)
        {
            Type type = Type.GetType("NetCore.Captcha." + TypeOfExtendedCaptchaOptions);
            return GetOptions(type);
        }

        public static CaptchaOptions GetOptions(Type type)
        {
            CaptchaOptions result;
            string typeName = type.FullName;
            if (options.ContainsKey(typeName))
            {
                result = options[typeName];
            }
            else
            {
                result = (CaptchaOptions)Activator.CreateInstance(type);
                SetOptions(typeName, result);
            }
            return result;
        }
        public static void SetOptions<T>(T Options) where T : CaptchaOptions
        {
            options.Add(typeof(T).FullName, Options);
        }
        public static void SetOptions(string typeName, CaptchaOptions Options)
        {
            options.Add(typeName, Options);
        }
    }
}
