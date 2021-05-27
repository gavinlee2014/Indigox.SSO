using Indigox.SSO.Interface;
using Indigox.SSO.Logs;
using Indigox.SSO.Services;

namespace Indigox.SSO.Audit
{
    public class TicketGrantingTicketAudit
    {
        public void AuditGrantTicketGrantingTicketAudit( ITicketGrantingTicket ticketGrantingTicket )
        {
            string user = ticketGrantingTicket.Authentication.Principal.UserName;
            Loggin.RecordLogin(user, ticketGrantingTicket.IP, ActionType.Login);
            //Log.Info( string.Format( "Grant tgt for user {0}.", user ) );
        }
    }
}