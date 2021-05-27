using System;
using System.Web;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client.Web
{
    public class SSOModules : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init( HttpApplication application )
        {
            Log.Debug( "SSO Module loading..." );
            application.AuthenticateRequest += new EventHandler( AuthenticateRequest );
            application.Error += new EventHandler( Error );
            application.LogRequest += new EventHandler(LogRequest);
            application.PreSendRequestContent += new EventHandler(PreSendRequestContent);
            Log.Debug( "SSO Module loaded." );
        }

        private void PreSendRequestContent(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            Log.Debug("SSOModules PreSendRequestContent " + context.Response.StatusCode + context.Request.RawUrl);
        }

        private void LogRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            Log.Debug("SSOModules LogRequest " + context.Response.StatusCode + context.Request.RawUrl);
            if (context.AllErrors != null)
            {
                foreach (var err in context.AllErrors)
                {
                    Log.Debug("SSOModules LogRequest error " + err.Message
                        + "\r\n" + err.StackTrace + "\r\n" + context.Request.RawUrl);
                }
            }
        }

        private void AuthenticateRequest( object sender, EventArgs e )
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            Log.Debug("SSOModules AuthenticateRequest " + context.Request.RawUrl);

            ValidateService validateService = new ValidateService( context );
            AccessTicketStorage accessTicketStorage = new AccessTicketStorage( context );
            if (IsIgnorePage(context))
            {
                return;
            }

            AccessTicket accessTicket = accessTicketStorage.Load();
            if ( accessTicket == null )
            {
                Log.Debug( "Access ticket is not exist, sign out immediately." );
                validateService.SignOut();

                RedirectToLoginPage( context );
            }
            else if ( AccessTicketManager.Instance.IsExpired( accessTicket ) )
            {
                Log.Debug( "Access ticket is already exist, but it is expired, sign out immediately." );
                validateService.SignOut();

                Log.Debug( "Reset access ticket [" + accessTicket.ID + "] not expired." );
                //AccessTicketManager.Instance.RemoveExpired( accessTicket.ID );

                RedirectToLoginPage( context );
            }
        }

        private void Error( object sender, EventArgs e )
        {
            Log.Debug("sso module in Error method");
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;

            Exception error = context.Error;
            Log.Debug("clear context error.");

            //context.ClearError();
            //context.Server.ClearError();

            if ( error is HttpUnhandledException )
            {
                error = error.InnerException;
            }
            else if ( error is UnauthorizedAccessException )
            {
                Log.Debug( "Catched UnauthorizedAccessException." );
                Log.Debug("Error : " + error.ToString());
                context.ClearError();
                ValidateService service = new ValidateService( context );
                service.RedirectToSSOLogin();
            }
            else
            {
                Log.Debug( "Catched exception, but it is not UnauthorizedAccessException, it is " + error.GetType().Name + "." );
                Log.Debug("Catched exception " + error.Message + "\r\n" + error.StackTrace);
            }
        }

        private void RedirectToLoginPage( HttpContext context )
        {
            Log.Debug(" Redirect To LoginPage ..");
            string requestUrl = context.Request.Url.PathAndQuery;
            string loginUrl = Settings.Instance.LoginUrl;
            loginUrl = loginUrl.Replace( "{ReturnUrl}", HttpUtility.UrlEncode( requestUrl ) );
            Log.Debug(" Login Url is : " + loginUrl);

            context.Response.Redirect( loginUrl,false);
            context.ApplicationInstance.CompleteRequest();
        }

        private bool IsIgnorePage( HttpContext context )
        {
            string requestUrl = context.Request.Url.PathAndQuery;

            string[] ignorePaths = Settings.Instance.SSOModuleIgnorePath.Split( ';' );

            foreach ( var ignorePath in ignorePaths )
            {
                if ( string.IsNullOrEmpty( ignorePath ) )
                {
                    continue;
                }
                if ( requestUrl.StartsWith( ignorePath, StringComparison.CurrentCultureIgnoreCase ) )
                {
                    return true;
                }
            }

            return false;
        }
    }
}