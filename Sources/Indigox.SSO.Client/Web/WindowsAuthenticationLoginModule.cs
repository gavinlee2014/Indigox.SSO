using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using Indigox.SSO.Client.Util;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security;

namespace Indigox.SSO.Client.Web
{
    public class WindowsAuthenticationLoginModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init( HttpApplication app )
        {
            Log.Debug("WindowsAuthenticationLoginModule Module loading...");
            app.AuthenticateRequest += new EventHandler(AuthenticateRequest);
            app.Error += new EventHandler(Error);
            app.LogRequest += new EventHandler(LogRequest);
            app.PreSendRequestContent += new EventHandler(PreSendRequestContent);
            Log.Debug("WindowsAuthenticationLoginModule Module loaded.");
        }

        private void PreSendRequestContent(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            Log.Debug("WindowsAuthenticationLoginModule PreSendRequestContent " + context.Response.StatusCode + context.Request.RawUrl);
        }

        private void LogRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            Log.Debug("WindowsAuthenticationLoginModule LogRequest " + context.Response.StatusCode + context.Request.RawUrl);
        }

        private void Error(object sender, EventArgs e)
        {
            Log.Debug("Global exception catched");
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;

            Exception error = context.Error;
            Log.Debug("Global exception:" + error.Message + "\r\n" + error.StackTrace);
        }

        private void AuthenticateRequest(object source, EventArgs eventArgs)
        {
            Log.Debug("WindowsAuthenticationLoginModule AuthenticateRequest...");
            
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            Log.Debug("WindowsAuthenticationLoginModule AuthenticateRequest " + context.Request.RawUrl);
            //if (IsIgnorePage(context))
            //{
            //    Log.Debug("is ignore page. url: "+context.Request.Url.ToString());
            //    return;
            //}

            //try
            //{
            //    Privileges.EnablePrivilege(SecurityEntity.SE_INCREASE_QUOTA_NAME);
            //    Log.Debug("Enable Privilege succed.");
            //}
            //catch (Exception ex)
            //{
            //    Log.Debug("Enable Privilege fail. error msg: " + ex.ToString());
            //}

            IPrincipal principal = null;
                //context.Session["SSO_PRINCIPAL"] as IPrincipal;
            if (principal == null)
            {
                WindowsIdentity identity = null;

                // Log.Debug("LoginHandler begin process...");

                var request = context.Request;
                var response = context.Response;

                AccessTicketStorage accessTicketStorage = new AccessTicketStorage(context);
                AccessTicket accessTicket = accessTicketStorage.Load();
                if (accessTicket == null)
                {
                    return;
                }
                string[] userPrincipalNames = GetUserPrincipalName(accessTicket.UserName);
                foreach (string userPrincipalName in userPrincipalNames)
                {
                    try
                    {
                        identity = new WindowsIdentity(userPrincipalName);
                        break;
                    }
                    catch (SecurityException ex)
                    {
                        Log.Debug("there is no user " + ex.Message);
                    }
                }
                identity = new WindowsIdentity(identity.Token, "NTLM", WindowsAccountType.System, true);
                //identity = new WindowsIdentity(identity.Token, "WindowsAuthentication", WindowsAccountType.System, true);
                Log.Debug("before set,current user is: " + (context.User == null ? "" : context.User.Identity.Name));
                Log.Debug("set current user: " + identity.Name);
                Log.Debug("identiy: " + identity.ImpersonationLevel);

                principal = new WindowsPrincipal(identity);
                //context.Session.Add("SSO_PRINCIPAL", principal);
            }
            context.User = principal;
        }

        private string[] GetUserPrincipalName( string userName )
        {
            List<String> upns = new List<string>();
            string[] domainNames = ConfigurationManager.AppSettings[ "DOMAIN_NAME" ].Split(';');
            foreach (string domainName in domainNames)
            {
                string upn = userName + "@" + domainName;
                Log.Debug("upn: " + upn);
                upns.Add(upn);
            }

            return upns.ToArray();
        }
        private bool IsIgnorePage(HttpContext context)
        {
            string requestUrl = context.Request.Url.PathAndQuery;

            string[] ignorePaths = Settings.Instance.SSOModuleIgnorePath.Split(';');

            foreach (var ignorePath in ignorePaths)
            {
                if (string.IsNullOrEmpty(ignorePath))
                {
                    continue;
                }
                if (requestUrl.StartsWith(ignorePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}