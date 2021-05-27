using System;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Tickets
{
    public class ClientToServiceTicket : AbstractTicket, IClientToServiceTicket
    {
        private ServiceTicket serviceTicket;

        public ServiceTicket ServiceTicket
        {
            get { return serviceTicket; }
            set { serviceTicket = value; }
        }

        public ClientToServiceTicket( string id, ServiceTicket serviceTicket )
            : base( id, null )
        {
            this.serviceTicket = serviceTicket;
            this.TicketGrantingTicket = serviceTicket.TicketGrantingTicket;
        }

        IServiceTicket IClientToServiceTicket.ServiceTicket
        {
            get { return ServiceTicket; }
        }
    }
}