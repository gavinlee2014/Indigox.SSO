using System;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Indigox.SSO.Client
{
    [WebService(Name="LogoutService",Namespace = "http://sso.indigox.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LogoutServiceClient : SoapHttpClientProtocol
    {
        public LogoutServiceClient(string url)
        {
            this.Url = url;
            this.Credentials = System.Net.CredentialCache.DefaultCredentials;
        }

        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sso.indigox.net/Logout", RequestNamespace = "http://sso.indigox.net/", ResponseNamespace = "http://sso.indigox.net/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Logout(string ticketGrantingTicketID)
        {
            return (bool)this.Invoke("Logout", new object[] { ticketGrantingTicketID })[0];
        }
    }
}