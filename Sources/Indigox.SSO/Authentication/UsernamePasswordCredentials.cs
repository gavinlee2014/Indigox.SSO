using System;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Authentication
{
    public class UsernamePasswordCredentials : ICredentials
    {
        public UsernamePasswordCredentials( string username, string password )
        {
            this.UserName = username;
            this.Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}