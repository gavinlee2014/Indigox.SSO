using System;
using System.Text;
using System.Web;
using Indigox.Common.Logging;
using Indigox.SSO.Authentication;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Application.Web
{
    public class BasicAuthLoginPage : BaseLoginPage
    {
        private ICredentials GetCredentials()
        {
            Log.Debug( "GetCredentials..." );
            var request = HttpContext.Current.Request;
            ICredentials credential = null;
            string authHeader = request.Headers[ "Authorization" ];
            if ( authHeader != null )
            {
                Log.Debug( "request header Authorization: " + authHeader );
                string[] paramters = authHeader.Split( ' ' );
                string authType = paramters[ 0 ];
                if ( authType == "Basic" )
                {
                    string cipherText = Encoding.UTF8.GetString( Convert.FromBase64String( paramters[ 1 ] ) );
                    string[] userInfo = cipherText.Split( ':' );
                    string userName = userInfo[ 0 ].Split( '\\' )[ 1 ];
                    string password = userInfo[ 1 ];
                    credential = new UsernamePasswordCredentials( userName, password );
                    Log.Debug( "Credentials : username: " + userName + ", password: " + password );
                }
            }
            else
            {
                Log.Debug( "no Authorization header" );
            }
            return credential;
        }

        protected void BasicAuthenticationLogin()
        {
            Log.Debug( "BasicAuthenticationLogin... " );
            ITicketGrantingTicket tgt = null;
            IClientToServiceTicket cst = null;
            TicketGrantingTicketStorage tgtStorage = new TicketGrantingTicketStorage( Context );
            UsernamePasswordCredentials credential = GetCredentials() as UsernamePasswordCredentials;
            try
            {
                IAuthentication authentication = new Authentication.Authentication( new Principal( credential.UserName ) );
                ITicketGrantingTicket oldTgt = tgtStorage.Load();
                if ( oldTgt != null )
                {
                    CentralAuthenticationService.Instance.Logout( oldTgt );
                }

                tgt = CentralAuthenticationService.Instance.CreateTicketGrantingTicket( authentication );
                cst = CentralAuthenticationService.Instance.GrantClientToServiceTicket( tgt, Service );
            }
            catch ( Exception ex )
            {
                Log.Debug( "Get Credential fail. exception: " + ex.Message );
                return; // show login page
            }

            tgtStorage.Save( tgt );

            string redirectUrl = GetRedirectToServiceUrl( cst );

            Log.Debug( "End BasicAuthenticationLogin . RedirectToService..." );
            Response.Redirect( redirectUrl,false);
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            if (IsPostBack||IsExplicitLoginRequest()){return;}
 
            TicketGrantingTicketStorage tgtStorage = new TicketGrantingTicketStorage( Context );
            ITicketGrantingTicket tgt = tgtStorage.Load();
            if ( tgt == null )
            {
                Log.Debug( "page load get tgt is null , into BasicAuthenticationLogin method..." );
                BasicAuthenticationLogin();
            }
            Log.Debug( "page load get tgt is exists." );
            IClientToServiceTicket cst = CentralAuthenticationService.Instance.GrantClientToServiceTicket( tgt, Service );

            // tgt.Services is changed, need rewrite cookie.
            tgtStorage.Save( tgt );

            string redirectUrl = GetRedirectToServiceUrl( cst );
            Log.Debug( "End page load . RedirectToService..." );
            Response.Redirect( redirectUrl,false);
        }
    }
}