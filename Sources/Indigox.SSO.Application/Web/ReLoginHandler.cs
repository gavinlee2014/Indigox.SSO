using Indigox.SSO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Indigox.SSO.Application.Web
{
    public class ReLoginHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            TicketGrantingTicketStorage tgtStorage = new TicketGrantingTicketStorage(context);
            ITicketGrantingTicket tgt = tgtStorage.Load();
            if (tgt != null)
            {
                CentralAuthenticationService.Instance.Logout(tgt);
                tgtStorage.Remove();
            }
            string redirectUrl = GetRedirectUrl(context.Request);
            context.Response.Redirect(redirectUrl);
        }

        private string GetRedirectUrl(HttpRequest request)
        {
            string service = request.QueryString["service"];
            string returnUrl = request.QueryString["returnURL"];
            string url = "/";
            if (!(string.IsNullOrEmpty(service)))
            {
                url = "/validate.ashx?useFormLogin=1&service=" + service;
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                url += "&returnURL=" + returnUrl;
            }
            return url;
        }
    }
}
