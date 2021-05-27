using System;
using System.Web.UI.WebControls;
using Indigox.Common.Logging;
using Indigox.SSO.Authentication;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Application.Web
{
    public class DefaultLoginPage : BaseLoginPage
    {
        protected Label Info;
        protected Button Login;
        protected TextBox Password;
        protected TextBox UserName;

        protected void Login_Click( object sender, EventArgs e )
        {
            string username = Request.Form[ "UserName" ];
            string password = Request.Form[ "Password" ];

            if ( string.IsNullOrEmpty( username ) )
            {
                SetInfo( "用户名不能为空！" );
                return; // show login page
            }

            ITicketGrantingTicket tgt = null;
            IClientToServiceTicket cst = null;
            TicketGrantingTicketStorage tgtStorage = new TicketGrantingTicketStorage( Context );

            try
            {
                IAuthenticator authenticationService = new ADAuthenticator();
                ICredentials credential = new UsernamePasswordCredentials( username, password );
                IAuthentication authentication = authenticationService.Authenticate( credential );

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
                SetInfo( ex.Message );
                return; // show login page
            }

            tgtStorage.Save( tgt );

            string redirectUrl = GetRedirectToServiceUrl(cst);
            Log.Debug("End page load . RedirectToService...");
            Response.Redirect(redirectUrl,false);
        }

        private void SetInfo( string msg )
        {
            this.Info.Text = msg;
        }
    }
}