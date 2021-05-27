using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Indigox.SSO.Client
{
    public class PrivilegedAccessTicketService
    {
        private HttpContext context;

        public PrivilegedAccessTicketService(HttpContext context)
        {
            this.context = context;
        }

        public void Accredit(string userName)
        {
            AccessTicket accessTicket = new AccessTicket(Guid.NewGuid().ToString(), userName);
            AccessTicketStorage storage = new AccessTicketStorage(this.context);
            storage.Save(accessTicket);
        }
    }
}
