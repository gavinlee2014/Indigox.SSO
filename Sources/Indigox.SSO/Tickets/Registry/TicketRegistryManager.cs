using System;

namespace Indigox.SSO.Tickets.Registry
{
    public class TicketRegistryManager
    {
        private static TicketRegistryManager instance;

        private TicketRegistryManager()
        {
            this.serviceTicketRegistry = new TicketRegistry();
        }

        public static TicketRegistryManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new TicketRegistryManager();
                }
                return instance;
            }
        }

        private TicketRegistry serviceTicketRegistry;

        public TicketRegistry ServiceTicketRegistry
        {
            get { return serviceTicketRegistry; }
            set { serviceTicketRegistry = value; }
        }
    }
}