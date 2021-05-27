using System;
using Indigox.Common.Logging;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Audit
{
    public class ClientToServiceTicketAudit
    {
        public void AuditGetClientToServiceTicket( IClientToServiceTicket clientToServiceTicket )
        {
            string user = clientToServiceTicket.TicketGrantingTicket.Authentication.Principal.UserName;
            Log.Info( string.Format( "Get cst for user {0}.", user ) );
        }
    }
}