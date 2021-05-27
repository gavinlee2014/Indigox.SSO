using System;

namespace Indigox.SSO.Client
{
    [System.Web.Services.WebServiceBindingAttribute( Name = "TicketServiceSoap", Namespace = "http://sso.indigox.net/" )]
    public class SSOTicketServiceClient : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        public SSOTicketServiceClient()
        {
            this.Url = Settings.Instance.SSOTicketServiceUrl;
        }

        [System.Web.Services.Protocols.SoapDocumentMethodAttribute( "http://sso.indigox.net/ValidateServiceTicket", RequestNamespace = "http://sso.indigox.net/", ResponseNamespace = "http://sso.indigox.net/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped )]
        public bool ValidateServiceTicket( string ticketToken, string serviceID )
        {
            object[] results = this.Invoke( "ValidateServiceTicket", new object[] {
                        ticketToken,
                        serviceID} );
            return ( (bool)( results[ 0 ] ) );
        }
    }
}