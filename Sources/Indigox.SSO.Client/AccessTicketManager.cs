using System;
using System.Collections.Generic;

namespace Indigox.SSO.Client
{
    public class AccessTicketManager
    {
        private static AccessTicketManager instance = new AccessTicketManager();

        private AccessTicketManager()
        {
        }

        public static AccessTicketManager Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, AccessTicket> ExpiredAccessTickets = new Dictionary<string, AccessTicket>();

        public bool IsExpired( AccessTicket accessTicket )
        {
            return ExpiredAccessTickets.ContainsKey( accessTicket.ID );
        }

        public void SetExpired( string accessTicketID )
        {
            ExpiredAccessTickets.Add( accessTicketID, null );
        }

        public void RemoveExpired( string accessTicketID )
        {
            ExpiredAccessTickets.Remove( accessTicketID );
        }
    }
}