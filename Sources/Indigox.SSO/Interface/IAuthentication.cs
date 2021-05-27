using System;

namespace Indigox.SSO.Interface
{
    public interface IAuthentication
    {
        IPrincipal Principal { get; }
        DateTime AuthenticatedTime { get; }
    }
}