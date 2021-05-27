using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    public class ClientToServiceTicketValidateService
    {
        private HttpContext context;
        public ClientToServiceTicketValidateService(HttpContext context){
            this.context = context;
        }
        
        public ClientToServiceTicket ClientToServiceTicket { private set; get; }

        public bool Validate()
        {
            string clientServiceTicketToken = GetClientToServiceTicketToken();
            Log.Debug(context.Request.RawUrl);
            Log.Debug("validate and get CST...\r\n\tcst: " + clientServiceTicketToken);

            ServiceTicketValidateService STValidateService = new ServiceTicketValidateService();
            if (string.IsNullOrEmpty(clientServiceTicketToken))
            {
                return false;
            }

            try
            {
                Log.Debug("get cst from token...");
                ClientToServiceTicket tempCST = ClientToServiceTicket.GetFrom(clientServiceTicketToken);

                Log.Debug("validate cst.st...");
                if (tempCST != null && STValidateService.Validate(tempCST.ServiceTicket))
                {
                    ClientToServiceTicket = tempCST;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("ValidateClientToServiceTicketAndServiceTicket failed, because of " + ex.Message);
                return false;
            }

        }

        private string GetClientToServiceTicketToken()
        {
            string clientServiceTicketToken = context.Request.QueryString["cst"];
            return clientServiceTicketToken;
        }
    }
}
