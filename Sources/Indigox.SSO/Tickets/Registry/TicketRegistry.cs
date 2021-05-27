using System;
using System.Collections.Generic;
using Indigox.Common.Logging;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Tickets.Registry
{
    public class TicketRegistry
    {
        private Dictionary<string, ITicket> cache = new Dictionary<string, ITicket>();

        public void AddTicket( ITicket ticket )
        {
            Log.Debug( "Added ticket [" + ticket.ID + "] to registry." );

            this.cache.Add( ticket.ID, ticket );
        }

        public ITicket GetTicket( string ticketId )
        {
            if ( !this.cache.ContainsKey( ticketId ) )
            {
                Log.Debug( "Ticket [" + ticketId + "] not found in registry." );
                return null;
            }

            return this.cache[ ticketId ];
        }

        public void DeleteTicket( string ticketId )
        {
            Log.Debug( "Removing ticket [" + ticketId + "] from registry." );

            this.cache.Remove( ticketId );
        }
    }
}