using System;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Authentication
{
    public class Principal : IPrincipal
    {
        public Principal( string username )
        {
            this.UserName = username;
        }

        public string UserName { get; set; }
    }
}