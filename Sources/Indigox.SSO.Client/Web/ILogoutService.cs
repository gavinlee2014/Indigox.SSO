using System;
using System.Web.Services;

namespace Indigox.SSO.Client.Web
{
    [WebService(Namespace = "http://sso.indigox.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public interface ILogoutService
    {
        [WebMethod(Description = "注销")]
        bool Logout(string ticketGrantingTicketID);
    }
}