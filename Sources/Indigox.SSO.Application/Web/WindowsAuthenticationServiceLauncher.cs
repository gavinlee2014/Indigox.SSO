using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Indigox.Common.Utilities;
using Indigox.SSO.Services;

namespace Indigox.SSO.Application
{
    internal class WindowsAuthenticationServiceLauncher
    {
        private System.Web.UI.Page page;
        private string password;
        private string redirectUrl;
        private string username;

        public WindowsAuthenticationServiceLauncher( System.Web.UI.Page page, string redirectUrl, string username, string password )
        {
            this.page = page;
            this.username = username;
            this.password = password;
            this.redirectUrl = redirectUrl;
        }

        private IList<RegisteredService> WindowsAuthenticationServices
        {
            get
            {
                ServiceManager serviceManager = new ServiceManager();
                return serviceManager.GetWindowsAuthenticationServices();
            }
        }

        /// <returns>
        /// If no frame generated, return false; Else, return true.
        /// </returns>
        public void RegisterScript()
        {
            string framesHtml = null;
            string script = null;
            int framesCount = 0;

            GenerateFramesHtml( ref framesCount, ref framesHtml );
            GenerateScript( framesCount, ref script );

            page.ClientScript.RegisterStartupScript( page.GetType(), "FramesHtml", framesHtml, false );
            page.ClientScript.RegisterStartupScript( page.GetType(), "Script", script, true );
        }

        private void GenerateFramesHtml( ref int framesCount, ref string framesHtml )
        {
            IList<RegisteredService> windowsAuthenticationServices = this.WindowsAuthenticationServices;
            StringBuilder htmlBuilder = new StringBuilder();
            string secertKey = Settings.Instance.WindowsAuthenticationSecertKey;
            string cipherUserName = HttpUtility.UrlEncode( DESCrypt.Encrypt( username, secertKey ) );
            string cipherPassword = HttpUtility.UrlEncode( DESCrypt.Encrypt( password, secertKey ) );
            cipherUserName = HttpUtility.UrlEncode( username );
            cipherPassword = HttpUtility.UrlEncode( password );

            framesCount = 0;

            foreach ( RegisteredService service in windowsAuthenticationServices )
            {
                if ( string.IsNullOrEmpty( service.AccessLoginUrl ) )
                {
                    continue;
                }

                string frameHtml = string.Format( "<iframe style=\"display:none;\" src=\"{0}#username={1}&password={2}\"></iframe>",
                    service.AccessLoginUrl, cipherUserName, cipherPassword );
                htmlBuilder.Append( frameHtml );
                framesCount++;
            }

            framesHtml = htmlBuilder.ToString();
        }

        private void GenerateScript( int framesCount, ref string script )
        {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine( "var redirectUrl=\"" + redirectUrl + "\";" );
            scriptBuilder.AppendLine( "var framesCount=" + framesCount + ";" );
            scriptBuilder.AppendLine( "GoTo();" );

            /*
             * 使用 location.replace 方法之后，用户不能通过“前进”和“后退”来访问已经被替换的 URL。
             */
            scriptBuilder.AppendLine( @"
function GoTo() {
    if (framesCount == 0) { window.location.replace(redirectUrl); }
}
function callback(state) {
    if (state == 1) { framesCount--; }
    else { alert(""登录失败""); framesCount--; }
    GoTo();
}" );

            script = scriptBuilder.ToString();
        }
    }
}