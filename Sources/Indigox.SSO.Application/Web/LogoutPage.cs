using System;
using Indigox.SSO.Interface;
using Indigox.Common.Logging;

namespace Indigox.SSO.Application.Web
{
    public class LogoutPage : System.Web.UI.Page
    {
        private TicketGrantingTicketStorage tgtStorage;

        protected void Page_Load(object sender, EventArgs e)
        {
            tgtStorage = new TicketGrantingTicketStorage(Context);
            ITicketGrantingTicket tgt = tgtStorage.Load();
            if (tgt != null)
            {
                CentralAuthenticationService.Instance.Logout(tgt);
                tgtStorage.Remove();
            }

            string redirectUrl = GetRedirectUrl();

            LogoutBasicAuthentication(redirectUrl);
        }

        private string GetRedirectUrl()
        {
            string service = Request.QueryString["service"];
            string returnUrl = Request.QueryString["returnURL"];
            string url = "/";
            if (!(string.IsNullOrEmpty(service) || string.IsNullOrEmpty(returnUrl)))
            {
                url = "/validate.ashx?service=" + service + "&returnURL=" + returnUrl;
            }
            return url;
        }
        private string GetRedirectParam()
        {
            string service = Request.QueryString["service"];
            string returnUrl = Request.QueryString["returnURL"];
            string param = "/";
            if (!(string.IsNullOrEmpty(service) || string.IsNullOrEmpty(returnUrl)))
            {
                param = "service=" + service + "&returnURL=" + returnUrl;
            }
            return param;
        }
        private void RedirectFromLogoutPage(string url)
        {
            Response.Redirect(url, false);
        }

        private void LogoutBasicAuthentication(string url)
        {
            Context.User = null;
              string script = @"<html><head></head><body><script type='text/javascript'>
                    function createXMLObject() {
                      try {
                        if (window.XMLHttpRequest) {
                          xmlhttp = new XMLHttpRequest();
                        }
                        // code for IE
                        else if (window.ActiveXObject) {
                          xmlhttp=new ActiveXObject('Microsoft.XMLHTTP');
                        }
                      } catch (e) {
                        xmlhttp=false
                      }
                      return xmlhttp;
                    }
                    function clearAuthenticationCache(page) {
                      if (!page) page = '/';
                       
                      //try {
                          var xmlhttp = createXMLObject();
                          
                          xmlhttp.open('GET', page, false, '{{username}}', '1');
                          xmlhttp.setRequestHeader('Authorization','Basic ZGRkOmFhYWE=');
                          xmlhttp.send('');
                      //} catch(e) {
                        //return;
                      //}
                    }
                    clearAuthenticationCache('/WindowsAuthenticationHandler.ashx?action=logout');
                    document.execCommand('ClearAuthenticationCache');
                    location.replace('{{redirectURL}}');
                </script></body></html>
                ".Replace("{{redirectURL}}", url)
                 .Replace("{{username}}", "excegroup\\\\" );
                 //.Replace("{{username}}", Context.User.Identity.Name.Replace("\\","\\\\")) ;
            Log.Debug("LogoutBasicAuthentication。 Script: " + script);
            //Response.AppendHeader("Connection", "close");
            //Response.StatusCode = 401; // Unauthorized;
            //Response.Clear();
            Response.Write(script);
            Response.End();
        }

        private bool IsBasicAuth()
        {
            string httpAuth = this.Request.Headers["Authorization"];
            return !string.IsNullOrEmpty(httpAuth) && httpAuth.StartsWith("basic", StringComparison.OrdinalIgnoreCase);
        }
    }
}