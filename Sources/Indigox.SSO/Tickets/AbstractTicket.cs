using System;
using Indigox.SSO.Interface;
using Indigox.SSO.Util;

namespace Indigox.SSO.Tickets
{
    public abstract class AbstractTicket : ITicket
    {
        private string id;
        private string ip;
        private bool isExpired;
        private TicketGrantingTicket ticketGrantingTicket;
        private DateTime createTime;
        private DateTime lastTimeUsed;
        private int countOfUses;


        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public bool IsExpired
        {
            get { return isExpired; }
            set { isExpired = value; }
        }

        public TicketGrantingTicket TicketGrantingTicket
        {
            get { return ticketGrantingTicket; }
            set { ticketGrantingTicket = value; }
        }

        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }

        public DateTime LastTimeUsed
        {
            get { return lastTimeUsed; }
            set { lastTimeUsed = value; }
        }

        public int CountOfUses
        {
            get { return countOfUses; }
            set { countOfUses = value; }
        }


        ITicketGrantingTicket ITicket.TicketGrantingTicket
        {
            get { return this.ticketGrantingTicket; }
        }

        public AbstractTicket( string id, TicketGrantingTicket ticket )
        {
            this.id = id;
            this.createTime = DateTime.Now;
            this.lastTimeUsed = DateTime.Now;
            this.ticketGrantingTicket = ticket;
            this.ip = IPHelper.GetIPAddress();
        }
    }
}