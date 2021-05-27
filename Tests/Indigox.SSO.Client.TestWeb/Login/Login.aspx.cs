using System;

namespace Indigox.SSO.Client.TestWeb.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            ValidateService validateService = new ValidateService( Context );
            Assertion assertion = validateService.Validate();
            string userName = assertion.UserName;
            SetAuthCookie( userName );
            RedirectFromLoginPage( userName );
        }

        /// <summary>
        /// 获取到用户名之后，从登录页跳转的资源页面
        /// </summary>
        protected virtual void RedirectFromLoginPage( string userName )
        {
            //TODO: implement
        }

        /// <summary>
        /// 设置 auth cookie （由接入系统实现）。
        /// </summary>
        /// <remarks>
        /// 可由 SSOAuthentication.SignOut() 删除 auth cookie。
        /// </remarks>
        protected virtual void SetAuthCookie( string userName )
        {
            //TODO: implement
        }
    }
}