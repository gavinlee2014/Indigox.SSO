using System;
using System.Web;
using System.Web.Security;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    public class ValidateService
    {
        private AccessTicketStorage accessTicketStorage;
        private HttpContext context;
        private CookieHelper cookieHelper;
        private HttpRequest request;
        private HttpResponse response;
        private ClientToServiceTicketValidateService CSTValidateService;

        /// <summary>
        /// constructor
        /// </summary>
        public ValidateService( HttpContext context )
        {
            this.context = context;
            this.request = context.Request;
            this.response = context.Response;
            this.cookieHelper = new CookieHelper( context );
            this.accessTicketStorage = new AccessTicketStorage( context );
            this.CSTValidateService = new ClientToServiceTicketValidateService(context);
        }

        public Assertion ValidateAT()
        {
            AccessTicket accessTicket = accessTicketStorage.Load();
            if ( accessTicket == null )
            {
                Log.Debug( "Access ticket is not exist, should sign out immediately." );
                return null;
            }

            if ( AccessTicketManager.Instance.IsExpired( accessTicket ) )
            {
                Log.Debug( "Access ticket is already exist, but it is expired, should sign out immediately." );
                AccessTicketManager.Instance.RemoveExpired( accessTicket.ID );
                return null;
            }
            
            Assertion assertion = new Assertion();
            assertion.UserName = accessTicket.UserName;
            assertion.ValidateTime = DateTime.Now;

            Log.Debug( "Access ticket is already exist, validate successfully - " + assertion.UserName );
            return assertion;
            
        }

        public Assertion ValidateCST()
        {
            if ( !CSTValidateService.Validate() )
            {
                return null;
            }

            ClientToServiceTicket cst = CSTValidateService.ClientToServiceTicket;
            
            AccessTicket accessTicket = GrantAccessTicket( cst, cst.UserName );
            accessTicketStorage.Save(accessTicket);

            Assertion assertion = new Assertion();
            assertion.UserName = accessTicket.UserName;
            assertion.ValidateTime = DateTime.Now;

            Log.Debug( "ClientToServiceTicket is already exist, validate successfully - " + assertion.UserName );

            return assertion;
        }
        /// <summary>
        /// 认证用户身份
        /// </summary>
        public Assertion Validate()
        {
            // check accessTicket
            Assertion assertion;
            assertion = ValidateAT();
            if ( assertion != null )
            {
                return assertion;
            }

            assertion = ValidateCST();
            if ( assertion != null )
            {
                return assertion;
            }

            //GoToSSOLogin();
            SignOut();
            throw new UnauthorizedAccessException();
        }

        /// <summary>
        /// 删除 cookies，可配置 appSettings 的 AUTH_COOKIES 设置删除哪些 cookie，多个值用分号(;)分隔
        /// </summary>
        public void SignOut()
        {
            Log.Debug( "Remove auth cookie..." );
            RemoveAuthCookie();
            accessTicketStorage.Remove();
        }

        /// <summary>
        /// 删除 auth cookie
        /// </summary>
        protected virtual void RemoveAuthCookie()
        {
            string authCookies = Settings.Instance.AuthCookies;
            if ( !string.IsNullOrEmpty( authCookies ) )
            {
                foreach ( string authCookie in authCookies.Split( ';' ) )
                {
                    Log.Debug(" Remove Auth Cookie [" + authCookie + "]");
                    cookieHelper.Remove( authCookie );
                }
            }
            FormsAuthentication.SignOut();
        }

        private string GetSSOLoginUrl()
        {
            string clientLoginUrl = request.Url.OriginalString;
            Log.Debug("clientLoginUrl:" + clientLoginUrl);
            Log.Debug("url encoded clientLoginUrl:" + HttpUtility.UrlEncode(clientLoginUrl));
            string ssoLoginUrl = string.Format( "{0}?returnURL={1}&service={2}", Settings.Instance.SSOLoginUrl,
                HttpUtility.UrlEncode( clientLoginUrl ), Settings.Instance.ServiceID );

            // asp.net default max url length is 2048
            if ( ssoLoginUrl.Length > 4096 )
            {
                throw new ApplicationException("ssoLoginUrl length(" + ssoLoginUrl.Length  + ") is too large.");
            }

            return ssoLoginUrl;
        }

        public void RedirectToSSOLogin()
        {
            string ssoLoginUrl = GetSSOLoginUrl();
            Log.Debug( "GoToSSOLogin...\r\n\turl:" + ssoLoginUrl );
            response.Redirect( ssoLoginUrl,false);
            //context.ApplicationInstance.CompleteRequest();
        }

        private AccessTicket GrantAccessTicket( ClientToServiceTicket clientServiceTicket, string userName )
        {
            AccessTicket newAccessTicket = new AccessTicket( clientServiceTicket.TicketGrantingTicketID, userName );
            return newAccessTicket;
        }

    }
}