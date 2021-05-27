using System;

namespace Indigox.SSO.Interface
{
    public interface ITicket
    {
        string ID { get; }

        bool IsExpired { get; }

        ITicketGrantingTicket TicketGrantingTicket { get; }

        DateTime CreateTime { get; set; }

        DateTime LastTimeUsed { get; set; }

        int CountOfUses { get; }

        string IP { get; set; }
    }
}