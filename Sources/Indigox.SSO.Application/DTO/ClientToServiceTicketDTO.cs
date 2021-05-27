using System;
using Indigox.Common.Utilities;
using Indigox.SSO.Application.Util;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Application.DTO
{
    public class ClientToServiceTicketDTO
    {
        public ClientToServiceTicketDTO()
        {
        }

        public ClientToServiceTicketDTO( IClientToServiceTicket ticket )
        {
            this.FromTicket( ticket );
        }

        private void FromTicket( IClientToServiceTicket ticket )
        {
            this.id = ticket.ID;

            //this.ipAddress = ticket.IPAddress
            this.userName = ticket.TicketGrantingTicket.Authentication.Principal.UserName;
            this.serviceTicket = DESCrypt.Encrypt( ticket.ServiceTicket.ID, Settings.Instance.SecretKey );
            this.ticketGrantingTicketID = ticket.ServiceTicket.TicketGrantingTicket.ID;
            this.createTime = ticket.CreateTime;
        }

        private string id;
        private string ipAddress;
        private string userName;
        private string serviceTicket;
        private string ticketGrantingTicketID;
        private DateTime createTime;

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

        public string GetToken( string secretKey )
        {
            return TicketSerializer.Serialize<ClientToServiceTicketDTO>( this, secretKey );
        }
    }
}