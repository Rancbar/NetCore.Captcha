#if NETCORE
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha.Extensions
{
    public static class CoreIHtmlHelperExtension
    {
        private static IUrlHelper getUrlHelper(IHtmlHelper htmlHelper)
        {
            var urlHelperFactory = (IUrlHelperFactory)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            return urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);
        }

        public static string Action(this IHtmlHelper helper)
        {
            return getUrlHelper(helper).Action();
        }
        public static string Action(this IHtmlHelper helper, string action)
        {
            return getUrlHelper(helper).Action(action);
        }
        public static string Action(this IHtmlHelper helper, string action, object values)
        {
            return getUrlHelper(helper).Action(action, values);
        }
        public static string Action(this IHtmlHelper helper, string action, string controller)
        {
            return getUrlHelper(helper).Action(action, controller);
        }
        public static string Action(this IHtmlHelper helper, string action, string controller, object values)
        {
            return getUrlHelper(helper).Action(action, controller, values);
        }
        public static string Action(this IHtmlHelper helper, string action, string controller, object values, string protocol)
        {
            return getUrlHelper(helper).Action(action, controller, values, protocol);
        }
        public static string Action(this IHtmlHelper helper, string action, string controller, object values, string protocol, string host)
        {
            return getUrlHelper(helper).Action(action, controller, values, protocol, host);
        }
        public static string Action(this IHtmlHelper helper, string action, string controller, object values, string protocol, string host, string fragment)
        {
            return getUrlHelper(helper).Action(action, controller, values, protocol, host, fragment);
        }
    }
}
#endif
