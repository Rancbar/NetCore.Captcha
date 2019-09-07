#if NETCORE || NETFULL
#if NETCORE
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Internal;
#elif NETFULL
using System.Web.Mvc;
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.Captcha.Validation
{
    public class CaptchaActionValidator
#if NETCORE
        : Attribute, IActionFilter
#elif NETFULL
        : ActionFilterAttribute
#endif
    {
        string invalidMessage;
        public CaptchaActionValidator(string invalidMessage = "Invalid captcha")
        {
            this.invalidMessage = invalidMessage;
        }

#if NETCORE
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
#elif NETFULL
        override
#endif
        public void OnActionExecuting(ActionExecutingContext context)
        {
#if NETFULL
            var modelState = context.Controller.ViewData.ModelState;
#elif NETCORE
            var modelState = context.ModelState;
#endif
            string tracker = context.HttpContext.Request.Form[Captcha.CaptchaTracker];
            string result = context.HttpContext.Request.Form[Captcha.CaptchaInputText];

            if (null == tracker)
            {
                modelState.AddModelError(
                    Captcha.CaptchaInputText,
                    "Cannot validate the captcha. Cannot find captcha data here."
                );
                return;
            }
            if (null == result)
            {
                modelState.AddModelError(
                    Captcha.CaptchaInputText,
                    string.Format(System.Globalization.CultureInfo.CurrentCulture, "Unknown property {0}", new[] { Captcha.CaptchaInputText })
                );
                return;
            }
            if (!Captcha.validate(tracker, result))
            {
                modelState.AddModelError(Captcha.CaptchaInputText, invalidMessage);
            }
        }
    }
}
#endif
