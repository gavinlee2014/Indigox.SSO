using System;
using System.Collections.Generic;
using Indigox.Common.Logging;
using Indigox.Common.Utilities;
using Indigox.SSO.Audit;
using Indigox.SSO.Client;
using Indigox.SSO.Interface;
using Indigox.SSO.Tickets;
using Indigox.SSO.Tickets.Registry;
using Indigox.SSO.Services;
using Indigox.SSO.Logs;

namespace Indigox.SSO
{
    public class CentralAuthenticationService : ICentralAuthenticationService
    {
        private static CentralAuthenticationService instance = new CentralAuthenticationService();

        public static CentralAuthenticationService Instance
        {
            get
            {
                return instance;
            }
        }

        public ITicketGrantingTicket CreateTicketGrantingTicket(IAuthentication authentication)
        {
            ArgumentAssert.NotNull(authentication, "authentication");

            Log.Debug("CreateTicketGrantingTicket...\r\n\tUserName:" + authentication.Principal.UserName);
            ITicketGrantingTicket ticketGrantingTicket = new TicketGrantingTicket(Guid.NewGuid().ToString(), (Authentication.Authentication)authentication);

            TicketGrantingTicketAudit audit = new TicketGrantingTicketAudit();
            audit.AuditGrantTicketGrantingTicketAudit(ticketGrantingTicket);

            return ticketGrantingTicket;
        }

        public IClientToServiceTicket GrantClientToServiceTicket(ITicketGrantingTicket ticketGrantingTicket, IRegisteredService service)
        {
            ArgumentAssert.NotNull(ticketGrantingTicket, "ticketGrantingTicket");
            ArgumentAssert.NotNull(service, "service");

            Log.Debug("GrantClientToServiceTicket...\r\n\tServiceID:" + service.ServiceID);
            IExpirationPolicy expirationPolicy = null;
            IServiceTicket serviceTicket = ticketGrantingTicket.GetServiceTicket(service, expirationPolicy);
            TicketRegistryManager.Instance.ServiceTicketRegistry.AddTicket(serviceTicket);
            IClientToServiceTicket clientToServiceTicket = ticketGrantingTicket.GetClientToServiceTicket(serviceTicket, expirationPolicy);

            ClientToServiceTicketAudit audit = new ClientToServiceTicketAudit();
            audit.AuditGetClientToServiceTicket(clientToServiceTicket);

            (ticketGrantingTicket as TicketGrantingTicket).AddRegiestedService(service);

            return clientToServiceTicket;
        }

        public void ValidateServiceTicket(string serviceTicketID, IRegisteredService registeredService)
        {
            ArgumentAssert.NotEmpty(serviceTicketID, "serviceTicketID");
            ArgumentAssert.NotNull(registeredService, "registeredService");

            Log.Debug("ValidateServiceTicket...");
            try
            {
                ServiceTicket serviceTicket = (ServiceTicket)TicketRegistryManager.Instance.ServiceTicketRegistry.GetTicket(serviceTicketID);

                if (serviceTicket == null)
                {
                    throw new ApplicationException("ServiceTicket [" + serviceTicketID + "] does not exist.");
                }

                if (serviceTicket.IsExpired)
                {
                    throw new Exception("ServiceTicket [" + serviceTicketID + "] has expired.");
                }

                if (serviceTicket.Service.ID != registeredService.ID)
                {
                    throw new Exception("ServiceTicket [" + serviceTicketID + "] with service [" + serviceTicket.Service.ID + "] does not match supplied service [" + registeredService.ID + "]");
                }

                ServiceTicketAudit audit = new ServiceTicketAudit();
                audit.AuditValidateServiceTicket(serviceTicket);
            }
            finally
            {
                TicketRegistryManager.Instance.ServiceTicketRegistry.DeleteTicket(serviceTicketID);
            }
        }

        public void Logout(ITicketGrantingTicket ticketGrantingTicket)
        {
            ArgumentAssert.NotNull(ticketGrantingTicket, "ticketGrantingTicket");

            // Audit
            Log.Debug("logout tgt [" + ticketGrantingTicket.ID + "]");

            string user = ticketGrantingTicket.Authentication.Principal.UserName;
            Loggin.RecordLogin(user, ticketGrantingTicket.IP, ActionType.Logout);

            IList<IRegisteredService> services = ticketGrantingTicket.Services;
            foreach (IRegisteredService service in services)
            {
                Log.Debug("--注销接入的系统： " + service.ServiceID);
                if (string.IsNullOrEmpty(service.LoginOutUrl))
                {
                    Log.Debug("    service [" + service.ServiceID + "] does not have LoginOutUrl.");
                    continue;
                }

                LogoutServiceClient client = new LogoutServiceClient(service.LoginOutUrl);
                try
                {
                    client.Logout(ticketGrantingTicket.ID);
                    Log.Debug("    logout service [" + service.ServiceID + "] done.");
                }
                catch (Exception ex)
                {
                    Log.Debug("    logout service [" + service.ServiceID + "] fail. ");
                    Log.Debug("    logout error : " + ex.ToString());
                    continue;
                }
            }
        }
    }
}