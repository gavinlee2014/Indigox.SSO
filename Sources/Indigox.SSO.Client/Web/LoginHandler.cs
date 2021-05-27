using System;
using System.Web;
using System.Web.Security;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.Web
{
    public class LoginHandler : IHttpHandler
    {
        private ValidateService validateService;
        private HttpRequest request;
        private HttpResponse response;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest( HttpContext context )
        {
            Log.Debug( "LoginHandler begin process..." );
            //调用SSOClient提供的接口判断用户是否登录
            this.request = context.Request;
            this.response = context.Response;
            this.validateService = new ValidateService( context );

            //获取验证之后的用户名
            Assertion assertion = validateService.Validate();
            string userName = assertion.UserName;
            //业务系统设置Cookie
            SetAuthCookie( userName );
            //跳转到实际的资源页
            RedirectFromLoginPage( userName );
        }

        /// <summary>
        /// 获取到用户名之后，从登录页跳转的资源页面
        /// </summary>
        protected virtual void RedirectFromLoginPage( string userName )
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

        /// <summary>
        /// 设置 auth cookie
        /// </summary>
        protected virtual void SetAuthCookie( string userName )
        {
            FormsAuthentication.SetAuthCookie( userName, false );
        }
    }
}