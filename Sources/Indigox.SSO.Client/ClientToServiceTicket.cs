using System;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    public class ClientToServiceTicket
    {
        private string id;
        private string ipAddress;
        private string userName;
        private string serviceTicket;
        private DateTime createTime;
        private string ticketGrantingTicketID;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string ServiceTicket
        {
            get { return serviceTicket; }
            set { serviceTicket = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public string TicketGrantingTicketID
        {
            get { return ticketGrantingTicketID; }
            set { ticketGrantingTicketID = value; }
        }

        public static ClientToServiceTicket GetFrom( string token )
        {
            return TicketSerializer.Deserialize<ClientToServiceTicket>( token, Settings.Instance.SecretKey );
        }
    }
}