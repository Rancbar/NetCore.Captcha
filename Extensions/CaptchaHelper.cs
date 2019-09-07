using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if NETCORE
using NetCore.Captcha.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
#elif NETFULL
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
#endif

namespace NetCore.Captcha
{
    public static class CaptchaHtmlString
    {
#if NETCORE
        public static Captcha MathCaptcha(this IHtmlHelper htmlHelper, string refreshText, string inputText, string requiredText)
#elif NETFULL
        public static Captcha MathCaptcha(this HtmlHelper htmlHelper, string refreshText, string inputText, string requiredText)
#else
        public static Captcha MathCaptcha()
#endif
        {
            MathCaptcha result = new MathCaptcha();
#if NETCORE
            string genUrl = htmlHelper.Action("Generate", "DefaultCaptcha", new { tracker = result.GetTracker() }).ToString();
            string refreshUrl = htmlHelper.Action("Refresh", "DefaultCaptcha", new { tracker = result.GetTracker() }).ToString();
#elif NETFULL
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string genUrl = urlHelper.Action("Generate", "DefaultCaptcha", new { tracker = result.GetTracker() });//.ToHtmlString();
            string refreshUrl = urlHelper.Action("Refresh", "DefaultCaptcha", new { tracker = result.GetTracker() });//.ToHtmlString();
            //string genUrl = htmlHelper.Action("Generate", "DefaultCaptcha").ToHtmlString();
            //string refreshUrl = htmlHelper.Action("Refresh", "DefaultCaptcha").ToHtmlString();
#endif


#if NETFULL || NETCORE
            result.setRefreshText(refreshText);
            result.setInputText(inputText);
            result.setRequiredText(requiredText);
            result.setGenerateUrl(genUrl);
            result.setRefreshUrl(refreshUrl);
#endif

            return result;
        }
    }
}
