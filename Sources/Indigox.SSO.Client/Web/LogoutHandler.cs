using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.Web
{
    public class LogoutHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            CookieHelper cookieHelper = new CookieHelper(context);
            cookieHelper.Clear();
            string loginUrl =URLUtil.GetSiteRoot()+ Settings.Instance.LoginUrl;
            loginUrl = loginUrl.Replace( "{ReturnUrl}", HttpUtility.UrlEncode( "/" ) );
            context.Response.Redirect(Settings.Instance.SSOLogoutUrl + "?service=" + Settings.Instance.ServiceID + "&returnURL=" + HttpUtility.UrlEncode(loginUrl), false);
        }
    }
}
