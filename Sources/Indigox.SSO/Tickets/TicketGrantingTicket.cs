using System;
using Indigox.Common.Logging;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;
using System.Collections.Generic;

namespace Indigox.SSO.Tickets
{
    public class TicketGrantingTicket : AbstractTicket, ITicketGrantingTicket
    {
        private Authentication.Authentication authentication;
        
        public Authentication.Authentication Authentication
        {
            get { return authentication; }
            set { authentication = value; }
        }

        public TicketGrantingTicket( string id, Authentication.Authentication authentication )
            : base( id, null )
        {
            this.authentication = (Authentication.Authentication)authentication;
            this.Services = new List<IRegisteredService>();
        }

        public ServiceTicket GetServiceTicket( IRegisteredService service, IExpirationPolicy expirationPolicy )
        {
            Log.Debug( "GetServiceTicket..." );
            return new ServiceTicket( Guid.NewGuid().ToString(), (RegisteredService)service, this );
        }

        public void AddRegiestedService(IRegisteredService service)
        {
            if (!ServiceExistsed(service))
            {
                this.Services.Add(service);
                Log.Debug("有新系统接入SSO。 Service ID: " + service.ServiceID + "  。当前系统数：" + this.Services.Count);
            }
        }

        private bool ServiceExistsed(IRegisteredService service)
        {
            foreach (IRegisteredService s in this.Services)
            {
                if (s.ServiceID == service.ServiceID) return true;
            }
            return false;
        }

        public ClientToServiceTicket GetClientToServiceTicket( ServiceTicket serviceTicket, IExpirationPolicy expirationPolicy )
        {
            Log.Debug( "GetClientToServiceTicket..." );
            return new ClientToServiceTicket( Guid.NewGuid().ToString(), serviceTicket );
        }

        IAuthentication ITicketGrantingTicket.Authentication
        {
            get { return this.Authentication; }
        }

        IServiceTicket ITicketGrantingTicket.GetServiceTicket( IRegisteredService service, IExpirationPolicy expirationPolicy )
        {
            return this.GetServiceTicket( service, expirationPolicy );
        }

        IClientToServiceTicket ITicketGrantingTicket.GetClientToServiceTicket( IServiceTicket serviceTicket, IExpirationPolicy expirationPolicy )
        {
            return this.GetClientToServiceTicket( (ServiceTicket)serviceTicket, expirationPolicy );
        }

        public System.Collections.Generic.IList<IRegisteredService> Services
        {
            get;
            set;
        }

    }
}