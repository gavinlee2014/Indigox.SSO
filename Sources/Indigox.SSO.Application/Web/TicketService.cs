using System;
using System.Web.Services;
using Indigox.Common.Logging;
using Indigox.Common.Utilities;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;

namespace Indigox.SSO.Application.Web
{
    [WebService( Namespace = "http://sso.indigox.net/" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class TicketService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool ValidateServiceTicket( string ticketToken, string serviceID )
        {
            Log.Debug( string.Format( "ValidateServiceTicket...\r\n\tticketToken:{0}\r\n\tserviceID:{1}", ticketToken, serviceID ) );
            try
            {
                string serviceTicketID = DESCrypt.Dncrypt( ticketToken, Settings.Instance.SecretKey );

                ServiceManager serviceManager = new ServiceManager();
                IRegisteredService service = serviceManager.GetRegisteredService( serviceID );
                if ( service == null || !service.IsEnabled )
                {
                    throw new ApplicationException( "serviceID is unavaliable." );
                }

                CentralAuthenticationService kdc = new CentralAuthenticationService();
                kdc.ValidateServiceTicket( serviceTicketID, service );

                return true;
            }
            catch ( Exception ex )
            {
                // invalid service ticket
                Log.Debug( "Invalid service ticket: " + ex.Message );

                return false;
            }
        }
    }
}