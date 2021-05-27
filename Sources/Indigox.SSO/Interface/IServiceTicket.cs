using System;

namespace Indigox.SSO.Interface
{
    public interface IServiceTicket : ITicket
    {
        IRegisteredService Service { get; }
    }
}