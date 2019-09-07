using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

#if NETCORE
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NETFULL
using System.Web;
#endif

namespace NetCore.Captcha
{
    abstract public class Captcha
#if NETCORE
        : IHtmlContent
#elif NETFULL
        : IHtmlString
#endif
    {
        public CaptchaOptions options;
        public new static CaptchaOptions Options
        {
            get { return OptionContainer.GetOptions<CaptchaOptions>(); }
            set { OptionContainer.SetOptions(value); }
        }

        public static readonly string CaptchaTracker = "CaptchaDeText";
        public static readonly string CaptchaInputText = "CaptchaInputText";
        protected static long uniqueCaptchaVersion = 0;
        protected string type;

        //protected char delimiter;
        public string CaptchaQuestion { get; protected set; }
        public string CaptchaAnswer { get; protected set; }
        public long ValidationExpiration = 10 * 60;
        private string refreshText = "Generate";
        private string inputText = "";
        private string requiredText = "Please enter the captcha";
        internal static Captcha Error = new ErrorCaptcha();

        public static Captcha GenerateByTracker(string tracker)
        {
            var cleanTracker = new Tracker(tracker);

            return new DefaultCaptcha(cleanTracker.CaptchaQuestion, cleanTracker.CaptchaAnswer, cleanTracker.type);
        }

        public Captcha(CaptchaOptions options = null)
        {
            if (null == options) options = Options;
            this.options = options;
            System.Threading.Interlocked.Increment(ref uniqueCaptchaVersion);
        }
        public static Captcha RefreshByTracker(string tracker)
        {
            Type captchaType = Type.GetType("NetCore.Captcha." + new Tracker(tracker).type);
            return (Captcha)Activator.CreateInstance(captchaType);
        }

        public string GetTracker()
        {
            long unixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return new Tracker(CaptchaQuestion, CaptchaAnswer, type ?? this.GetType().Name, uniqueCaptchaVersion, unixTime).ToString();
        }

        public static bool validate(string tracker, string valueCheck)
        {
            Tracker _tracker = new Tracker(tracker);
            if (
                _tracker.CaptchaAnswer == valueCheck &&
                DateTimeOffset.FromUnixTimeSeconds(_tracker.unixTime).AddMinutes(10) > DateTimeOffset.Now &&
                Store.IsUsed(_tracker.uniqueV) == null
            )
            {
                Store.MakeUsed(_tracker.uniqueV);
                return true;
            }
            return false;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            writer.Write(ToHtmlString());
        }

        public string ToHtmlString()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<script type\"text/javascript\">");
            html.Append("$(function() {$('#ad28789dd95644eead8433790fd7bf01').show(); });");
            html.Append("function ______e36b4b87abf1445cac85e8c247c9f0ad________() {");
            html.Append("$('#ad28789dd95644eead8433790fd7bf01').hide();");
            html.Append("$.post(\"")
                .Append(refreshUrl)
                .Append("\", { t: $('#CaptchaDeText').val(), __m__: \"0\" }, function(){$('#ad28789dd95644eead8433790fd7bf01').show(); });");
            html.Append("return false;");
            html.Append("}");
            html.Append("</script>");
            html.Append("<br/>");
            if (null == generateUrl || string.Empty.Equals(generateUrl))
            {
                html.Append("<img id=\"CaptchaImage\" src=\"")
                    .Append(this.ToBase64())
                    .Append("\"/>");
            }
            else
            {
                html.Append("<img id=\"CaptchaImage\" src=\"")
                    .Append(generateUrl)
                    .Append("\"/>");
            }
            html.Append("<input id=\"")
                .Append(CaptchaTracker)
                .Append("\" name=\"")
                .Append(CaptchaTracker)
                .Append("\" type=\"hidden\" value=\"")
                .Append(GetTracker())
                .Append("\" />");
            html.Append("<br/>");
            html.Append("<a href=\"#CaptchaImage\" id=\"ad28789dd95644eead8433790fd7bf01\" onclick=\"______e36b4b87abf1445cac85e8c247c9f0ad________()\" style=\"display:none;\">")
                .Append(refreshText)
                .Append("</a>");
            html.Append("<br/>").Append("<br/>");
            html.Append("<input autocomplete=\"off\" autocorrect=\"off\" data-val=\"true\" data-val-required=\"");
            html.Append(requiredText).Append("\" id=\"").Append(CaptchaInputText).Append("\" name=\"").Append(CaptchaInputText).Append("\" type=\"text\" value=\"\" />");

            return html.ToString();
        }


        public void setGenerateUrl(string generateUrl)
        {
            this.generateUrl = generateUrl;
        }
        public void setRefreshUrl(string refreshUrl)
        {
            this.refreshUrl = refreshUrl;
        }
        public void setRefreshText(string refreshText)
        {
            this.refreshText = refreshText;
        }
        public void setInputText(string inputText)
        {
            this.inputText = inputText;
        }
        public void setRequiredText(string requiredText)
        {
            this.requiredText = requiredText;
        }


        private static Random rand = new Random((int)DateTime.Now.Ticks);
        private string generateUrl;
        private string refreshUrl;

        virtual public MemoryStream ToStream()
        {
            using (MemoryStream mem = new MemoryStream())
            using (Bitmap bmp = new Bitmap(130, 30))
            using (Graphics gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (options.Noisy)
                {
                    int i, r, x, y;
                    Pen pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(CaptchaQuestion, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);

                return mem;
            }
        }

        private class Tracker
        {
            public string CaptchaQuestion;
            public string CaptchaAnswer;
            public string type;
            public long uniqueV;
            public long unixTime;

            public Tracker(string tracker)
            {
                string cleanTracker = tracker.DecryptString();
                string[] parts = cleanTracker.Split('|');
                CaptchaQuestion = parts[3];
                CaptchaAnswer = parts[0];
                type = parts[1];
                uniqueV = long.Parse(parts[2]);
                unixTime = long.Parse(parts[4]);
            }

            public Tracker(
                string CaptchaQuestion,
                string CaptchaAnswer,
                string type,
                long uniqueCaptchaVersion,
                long unixTime
            )
            {
                this.CaptchaQuestion = CaptchaQuestion;
                this.CaptchaAnswer = CaptchaAnswer;
                this.type = type;
                this.uniqueV = uniqueCaptchaVersion;
                this.unixTime = unixTime;
            }

            override public string ToString()
            {
                string crypted = (CaptchaAnswer + "|" + type + "|" + uniqueV + "|" + CaptchaQuestion + "|" + unixTime).EncryptString();
                return crypted;
            }
        }

        private class DefaultCaptcha : Captcha
        {
            public DefaultCaptcha(string CaptchaQuestion, string CaptchaAnswer, string type) : base(OptionContainer.GetOptions(type+ "Options"))
            {
                this.CaptchaQuestion = CaptchaQuestion;
                this.CaptchaAnswer = CaptchaAnswer;
                this.type = type;
            }
        }

        private class ErrorCaptcha : Captcha
        {
            static ErrorCaptcha()
            {
                int width = 130;
                int height = 30;
                using (MemoryStream mem = new MemoryStream())
                using (Bitmap bmp = new Bitmap(width, height))
                using (Graphics gfx = Graphics.FromImage((Image)bmp))
                {
                    using (Pen blackPen = new Pen(Color.Red, 3))
                    {
                        gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, bmp.Width, bmp.Height));

                        // Add cross lines
                        gfx.DrawLine(blackPen, 0, 0, width, height);
                        gfx.DrawLine(blackPen, width, 0, 0, height);

                        //render as Jpeg
                        bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);

                        ErrorImageStream = mem;
                    }
                }
            }

            public static MemoryStream ErrorImageStream { get; }
            public ErrorCaptcha() : base()
            {
            }
            override public MemoryStream ToStream()
            {
                return ErrorImageStream;
            }
        }
    }
}
