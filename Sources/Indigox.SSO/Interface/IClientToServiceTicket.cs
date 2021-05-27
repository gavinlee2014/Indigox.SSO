using System;

namespace Indigox.SSO.Interface
{
    public interface IClientToServiceTicket : ITicket
    {
        IServiceTicket ServiceTicket { get; }
    }
}