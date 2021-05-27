using System;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;

namespace Indigox.SSO.Tickets
{
    public class ServiceTicket : AbstractTicket, IServiceTicket
    {


        private RegisteredService service;

        public RegisteredService Service
        {
            get { return service; }
            set { service = value; }
        }

        IRegisteredService IServiceTicket.Service
        {
            get { return this.Service; }
        }

        public ServiceTicket( string id, RegisteredService service, TicketGrantingTicket ticket )
            : base( id, ticket )
        {
            this.service = service;
        }
    }
}