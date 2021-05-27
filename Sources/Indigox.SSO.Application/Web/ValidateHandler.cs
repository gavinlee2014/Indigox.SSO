using System;
using System.Web;
using Indigox.SSO.Interface;
using Indigox.Common.Logging;
using Indigox.SSO.Services;
using System.Configuration;
using Indigox.SSO.Application.DTO;
using Indigox.SSO.Application.Util;

namespace Indigox.SSO.Application.Web
{
    public class ValidateHandler : IHttpHandler
    {
        protected IRegisteredService GetService(HttpRequest request)
        {
            string serviceID = request.QueryString["service"];

            IRegisteredService service;

            ServiceManager serviceManager = new ServiceManager();
            if (string.IsNullOrEmpty(serviceID))
            {
                service = serviceManager.GetDefaultRegisteredService();
            }
            else
            {
                service = serviceManager.GetRegisteredService(serviceID);
            }

            return service;
        }

        protected string GetDefaultRedirectUrl()
        {
            string redirectUrl = null;
            ServiceManager serviceManager = new ServiceManager();
            RegisteredService defaultRegisteredService = serviceManager.GetDefaultRegisteredService();
            if (defaultRegisteredService != null)
            {
                redirectUrl = defaultRegisteredService.LoginUrl;
            }
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = ConfigurationManager.AppSettings["DEFAULT_REDIRECT_URL"];
            }
            return redirectUrl;
        }

        protected virtual string GetRedirectToServiceUrl(IClientToServiceTicket cst, IRegisteredService service)
        {
            HttpRequest request = HttpContext.Current.Request;
            string cstToken = new ClientToServiceTicketDTO(cst).GetToken(service.SecretKey);
            Log.Debug("cstToken:" + cstToken);

            string redirectUrl = request.QueryString["returnURL"];
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = GetDefaultRedirectUrl();
            }

            if (redirectUrl.IndexOf('?') < 0)
            {
                redirectUrl = string.Format("{0}?cst={1}", redirectUrl, HttpUtility.UrlEncode(cstToken));
            }
            else
            {
                //redirectUrl = redirectUrl.Replace("&", HttpUtility.UrlEncode("&"));
                redirectUrl = string.Format("{0}&cst={1}", redirectUrl, HttpUtility.UrlEncode(cstToken));
            }
            Log.Debug("redirect to " + redirectUrl);

            // asp.net default max url length is 2048
            if (redirectUrl.Length > 2000)
            {
                throw new ApplicationException("redirectUrl length is too large.");
            }

            return redirectUrl;
        }

        protected virtual string GetLoginPageUrl()
        {
            HttpRequest request = HttpContext.Current.Request;
            if ((request.UserAgent != null) && (request.UserAgent.Contains("DingTalk")))
            {
                return string.Format("{0}?{1}", Settings.Instance.DingLoginUrl, request.QueryString);
            }
            else if (request.Params["useFormLogin"] != null)
            {
                return string.Format("{0}?{1}", Settings.Instance.FormLoginUrl, request.QueryString);
            }
            else
            {
                string baseUrl = URLUtil.GetSiteRoot();
                return string.Format("{0}{1}?{2}", baseUrl, Settings.Instance.LoginUrl, request.QueryString);
            }
        }     
        
        public bool IsReusable
        {
            get { return true; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            Log.Debug("validate handler processing...");
            TicketGrantingTicketStorage tgtStorage = new TicketGrantingTicketStorage(context);
            ITicketGrantingTicket tgt = tgtStorage.Load();
            if (tgt == null)
            {
                Log.Debug(" get tgt is null ,go to login page...");
                //跳转到登陆页
                string loginPageUrl=GetLoginPageUrl();
                Log.Debug(loginPageUrl);
                context.Response.Redirect(loginPageUrl,false);
                return;
            }
            Log.Debug(" get tgt is exists.");
            IRegisteredService service = GetService(context.Request);
            IClientToServiceTicket cst = CentralAuthenticationService.Instance.GrantClientToServiceTicket(tgt, service);

            // tgt.Services is changed, need rewrite cookie.
            tgtStorage.Save(tgt);

            string redirectUrl = GetRedirectToServiceUrl(cst,service);
            Log.Debug("validate handler ended . Redirect to service...");
            context.Response.Redirect(redirectUrl);
        }

    }
}
