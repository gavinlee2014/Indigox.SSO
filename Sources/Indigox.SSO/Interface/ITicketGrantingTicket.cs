using System;
using System.Collections.Generic;

namespace Indigox.SSO.Interface
{
    public interface ITicketGrantingTicket : ITicket
    {
        IList<IRegisteredService> Services { get;set; }

        IAuthentication Authentication { get; }

        IServiceTicket GetServiceTicket( IRegisteredService service, IExpirationPolicy expirationPolicy );

        IClientToServiceTicket GetClientToServiceTicket( IServiceTicket serviceTicket, IExpirationPolicy expirationPolicy );
    }
}