using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.SSO.Application.Web
{
    public class WindowsAuthenticationHandler : AbstractAuthenticationHandler
    {
        protected override string GetUserName()
        {
            string userName = context.User.Identity.Name;
            userName = userName.Substring(userName.IndexOf("\\") + 1);
            return userName;
        }
    }
}
