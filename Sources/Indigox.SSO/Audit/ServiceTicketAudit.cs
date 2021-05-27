using Indigox.SSO.Interface;
using Indigox.SSO.Services;

namespace Indigox.SSO.Audit
{
    public class ServiceTicketAudit
    {
        public void AuditValidateServiceTicket( IServiceTicket serviceTicket )
        {
            string user = serviceTicket.TicketGrantingTicket.Authentication.Principal.UserName;
            Loggin.RecordAccess(user, serviceTicket.IP, serviceTicket.Service);
        }
    }
}