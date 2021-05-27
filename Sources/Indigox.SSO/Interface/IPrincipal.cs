using System;

namespace Indigox.SSO.Interface
{
    public interface IPrincipal
    {
        string UserName { get; }
    }
}