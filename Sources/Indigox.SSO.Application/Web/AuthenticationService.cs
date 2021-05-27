using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using Indigox.SSO.Interface;
using Indigox.SSO.Authentication;

namespace Indigox.SSO.Application.Web
{
    [WebService( Namespace = "http://sso.indigox.net/" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class AuthenticationService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool Validate(string userName, string password, ref string error)
        {
            try
            {
                IAuthenticator authenticationService = new ADAuthenticator();
                ICredentials credential = new UsernamePasswordCredentials(userName, password);
                IAuthentication authentication = authenticationService.Authenticate(credential);
            }
            catch (Exception ex)
            {   
                error = ex.Message;
                return false;
            }
            
            return true;
        }
    }
}
