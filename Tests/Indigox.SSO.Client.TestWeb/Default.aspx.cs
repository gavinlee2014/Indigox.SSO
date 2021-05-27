using System;
using System.Web.Security;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.TestWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
        }

        protected void btnClearAuthCookie_clicked( object source, EventArgs e )
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnClearAccessTicket_clicked( object source, EventArgs e )
        {
            CookieHelper cookieHelper = new CookieHelper( Context );
            cookieHelper.Remove( Settings.Instance.AccessTicketCookieName );
            Response.Redirect( Request.Url.AbsoluteUri,false);
        }
    }
}