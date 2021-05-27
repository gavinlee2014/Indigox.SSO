using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;

namespace Indigox.SSO.Application.Web
{
    public class TrustedAuthenticationHandler: AbstractAuthenticationHandler
    {
        protected override string GetUserName()
        {
            string ssouserCipher = context.Request.QueryString["SSOUSER"];
            if (string.IsNullOrEmpty(ssouserCipher))
            {
                return null;
            }
            string ssouserPlain= Encoding.UTF8.GetString(Convert.FromBase64String(ssouserCipher));
            ssouserPlain = HttpUtility.UrlDecode(ssouserPlain);
            string seperator = "_x_x_x_";
            int seperatorIndex = ssouserPlain.IndexOf(seperator);
            if (seperatorIndex < 0)
            {
                return null;
            }
            return ssouserPlain.Substring(0, seperatorIndex);
        }
    }
}
