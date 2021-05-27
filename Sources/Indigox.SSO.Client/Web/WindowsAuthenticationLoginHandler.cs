using System;
using System.Web;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.Web
{
    public class WindowsAuthenticationLoginHandler : IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest( HttpContext context )
        {
            Log.Debug("WindowsAuthenticationLoginHandler begin...");
            this.request = context.Request;
            this.response = context.Response;

            Log.Debug("WindowsAuthenticationLoginHandler validateService begin...");
            ValidateService validateService = new ValidateService(context);

            Assertion assertion = validateService.Validate();
            string userName = assertion.UserName;

            RedirectFromLoginPage();
            Log.Debug("WindowsAuthenticationLoginHandler end...");
        }

        /// <summary>
        /// 获取到用户名之后，从登录页跳转的资源页面
        /// </summary>
        protected virtual void RedirectFromLoginPage()
        {
            Log.Debug( "RedirectFromLoginPage..." );

            string returnUrl = request.QueryString[ "ReturnUrl" ];//跳转URL
            if ( string.IsNullOrEmpty( returnUrl ) )
            {
                returnUrl = "/";
            }

            Log.Debug( "returnUrl: " + returnUrl );
            response.Redirect( returnUrl,false);
        }
    }
}