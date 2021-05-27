using System;
using System.Collections.Generic;
using Indigox.SSO.Application.Util;
using Indigox.SSO.Authentication;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;
using Indigox.SSO.Tickets;

namespace Indigox.SSO.Application.DTO
{
    public class TicketGrantingTicketDTO
    {
        public TicketGrantingTicketDTO()
        {
        }

        public TicketGrantingTicketDTO( ITicketGrantingTicket ticket )
        {
            this.FromTicket( ticket );
        }

        private ITicketGrantingTicket GetTicket()
        {
            Principal principal = new Principal( this.userName );
            Authentication.Authentication authentication = new Authentication.Authentication( principal );
            TicketGrantingTicket tgt = new TicketGrantingTicket( this.id, authentication );
            foreach ( var item in GetServices( this.serviceIDs ) )
            {
                tgt.Services.Add( item );
            }
            return tgt;
        }

        private void FromTicket( ITicketGrantingTicket ticket )
        {
            this.id = ticket.ID;

            //this.ipAddress = ticket.IPAddress
            this.userName = ticket.Authentication.Principal.UserName;
            this.createTime = ticket.CreateTime;
            this.serviceIDs = GetServiceIDs( ticket.Services );
        }

        private string GetServiceIDs( IList<IRegisteredService> iList )
        {
            List<string> ids = new List<string>();
            if ( iList != null )
            {
                foreach ( var item in iList )
                {
                    ids.Add( item.ServiceID );
                }
            }
            return String.Join( ";", ids.ToArray() );
        }

        private IList<IRegisteredService> GetServices( string serviceIDs )
        {
            List<IRegisteredService> services = new List<IRegisteredService>();
            string[] ids = serviceIDs.Split( new string[] { ";" }, StringSplitOptions.None );
            ServiceManager serviceManager = new ServiceManager();
            foreach ( var id in ids )
            {
                services.Add( serviceManager.GetRegisteredService( id ) );
            }
            return services;
        }

        private string id;
        private string ipAddress;
        private string userName;
        private DateTime createTime;
        private string serviceIDs;

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
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public string ServiceIDs
        {
            get { return serviceIDs; }
            set { serviceIDs = value; }
        }

        public static ITicketGrantingTicket GetTicket( string tokenText, string secretKey )
        {
            TicketGrantingTicketDTO ticket = TicketSerializer.Deserialize<TicketGrantingTicketDTO>( tokenText, secretKey );
            return ( ticket == null ) ? null : ticket.GetTicket();
        }

        public string GetToken( string secretKey )
        {
            return TicketSerializer.Serialize<TicketGrantingTicketDTO>( this, secretKey );
        }
    }
}