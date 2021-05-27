using System;
using System.Web.Services;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.Web
{
    [WebService( Namespace = "http://sso.indigox.net/" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class LogoutService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool Logout( string ticketGrantingTicketID )
        {
            Log.Debug( "call client logout method" );
            Log.Debug("Logout with TGT :" + ticketGrantingTicketID);
            string accessTicketID = ticketGrantingTicketID;
            AccessTicketManager.Instance.SetExpired( accessTicketID );
            return true;
        }
    }
}