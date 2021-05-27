using System;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Authentication
{
    public class Authentication : IAuthentication
    {
        public Authentication( Principal principal )
        {
            this.Principal = principal;
            this.AuthenticatedTime = DateTime.Now;
        }

        public DateTime AuthenticatedTime { get; set; }

        public Principal Principal { get; set; }

        IPrincipal IAuthentication.Principal
        {
            get { return this.Principal; }
        }
    }
}