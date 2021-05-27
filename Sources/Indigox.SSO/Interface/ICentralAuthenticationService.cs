using System;

namespace Indigox.SSO.Interface
{
    public interface ICentralAuthenticationService
    {
        ITicketGrantingTicket CreateTicketGrantingTicket( IAuthentication authentication );

        IClientToServiceTicket GrantClientToServiceTicket( ITicketGrantingTicket ticketGrantingTicket, IRegisteredService service );

        void ValidateServiceTicket( string serviceTicketID, IRegisteredService service );

        void Logout( ITicketGrantingTicket ticketGrantingTicket );
    }
}