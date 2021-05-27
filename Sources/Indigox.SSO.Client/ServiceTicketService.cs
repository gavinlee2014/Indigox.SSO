using System;
using System.Collections.Generic;
using System.Text;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    public class ServiceTicketValidateService
    {
        public bool Validate(string serviceTicketToken)
        {
            Log.Debug("ValidateServiceTicket...\r\n\tserviceTicketToken:" + serviceTicketToken);
            SSOTicketServiceClient valicator = new SSOTicketServiceClient();
            return valicator.ValidateServiceTicket(serviceTicketToken, Settings.Instance.ServiceID);
        }
    }
}
