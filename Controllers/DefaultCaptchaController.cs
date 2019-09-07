//#define NETFULL
#if NETFULL || NETCORE
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if NETCORE
using Microsoft.AspNetCore.Mvc;
#elif NETFULL
using System.Web.Mvc;
#endif

namespace NetCore.Captcha.Controllers
{
#if NETCORE
    [Route("api/[controller]")]
#endif
    public class DefaultCaptchaController: Controller
    {
        [HttpGet]
        public ActionResult Generate(string tracker)
        {
            Captcha captcha;
            try
            {
#if NETCORE
                String referer = HttpContext.Request.Headers["Referer"].ToString();
                Microsoft.AspNetCore.Http.HttpRequest req = HttpContext.Request;
                Uri rUri = new System.UriBuilder(referer).Uri;
                if (req.Host.Host == rUri.Host && req.Host.Port == rUri.Port && req.Scheme == rUri.Scheme && rUri.AbsolutePath == req.Path)
                {
#elif NETFULL
                Uri rUri = System.Web.HttpContext.Current.Request.UrlReferrer;
                Uri req = System.Web.HttpContext.Current.Request.Url;
                if (req.AbsolutePath == rUri.AbsolutePath)
                {
#endif
                    throw new InvalidOperationException();
                }

                captcha = Captcha.GenerateByTracker(tracker);
            }
            catch (Exception)
            {
                captcha = Captcha.Error;
            }

            return File(captcha.ToStream().GetBuffer(), System.Net.Mime.MediaTypeNames.Image.Jpeg);
        }

        public ActionResult Refresh(string tracker)
        {
#if NETCORE
            
            if (HttpContext.Request.Headers["x-requested-with"] != "XMLHttpRequest")
            {
                return this.Redirect(new System.UriBuilder(HttpContext.Request.Headers["Referer"].ToString()).Uri.AbsolutePath);
            }
#elif NETFULL
            if (!AjaxRequestExtensions.IsAjaxRequest(System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request))
            {
                return this.Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.AbsolutePath);
            }
#endif

            //(Captcha)Activator.CreateInstance()

            //HttpContext.Session.SetString("Captcha" + prefix, captcha.CaptchaAnswer);
            HttpContext.Response.ContentType = "application/x-javascript; charset=utf-8";
            Captcha captcha = Captcha.RefreshByTracker(tracker);
            return Content(
                "$('#CaptchaDeText').attr('value', '" + captcha.GetTracker() + "');" +
                "$('#CaptchaImage').attr('src', '" + captcha.ToBase64() + "');"
            );
        }
    }
}

#endif