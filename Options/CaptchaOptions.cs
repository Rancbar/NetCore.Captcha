using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha
{
    public class CaptchaOptions
    {
        public bool Noisy { get; private set; } = true;

        public CaptchaOptions() : this(true) { }
        public CaptchaOptions(bool Noisy = true)
        {
            this.Noisy = Noisy;
        }
    }
}
