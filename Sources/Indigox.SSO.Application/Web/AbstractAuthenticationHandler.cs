using System;
using System.Configuration;
using System.Web;
using Indigox.Common.Logging;
using Indigox.SSO.Application.DTO;
using Indigox.SSO.Authentication;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;

namespace Indigox.SSO.Application.Web
{
    public abstract class AbstractAuthenticationHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        protected abstract string GetUserName();

        protected HttpContext context;

        protected IRegisteredService service;

        protected ITicketGrantingTicket tgt;

        protected IClientToServiceTicket cst;

        public virtual void ProcessRequest( HttpContext context )
        {
            this.context = context;
            CreateTicketGrantingTicket();
            GrantClientToServiceTicket();
            SendClientToServiceTicket();
        }

        protected void CreateTicketGrantingTicket()
        {
            IAuthentication authentication = new Authentication.Authentication( new Principal( GetUserName() ) );
            tgt = CentralAuthenticationService.Instance.CreateTicketGrantingTicket( authentication );

            TicketGrantingTicketStorage storage = new TicketGrantingTicketStorage( context );
            storage.Save( tgt );
        }

        protected void GrantClientToServiceTicket()
        {
            cst = CentralAuthenticationService.Instance.GrantClientToServiceTicket( tgt, GetService() );
            TicketGrantingTicketStorage storage = new TicketGrantingTicketStorage( context );
            storage.Save( tgt );
        }

        protected void SendClientToServiceTicket()
        {
            string url = GetServiceRedirectUrl();
            if ( url == null )
            {
                url = GetDefaultRedirectUrl();
            }
            context.Response.Redirect(url, false); 
        }

        protected IRegisteredService GetService()
        {
            string serviceID = context.Request.QueryString[ "service" ];

            ServiceManager serviceManager = new ServiceManager();
            if ( string.IsNullOrEmpty( serviceID ) )
            {
                service = serviceManager.GetDefaultRegisteredService();
            }
            else
            {
                service = serviceManager.GetRegisteredService( serviceID );
            }

            return service;
        }

        protected string GetDefaultRedirectUrl()
        {
            string url = null;
            ServiceManager serviceManager = new ServiceManager();
            RegisteredService defaultRegisteredService = serviceManager.GetDefaultRegisteredService();
            if ( defaultRegisteredService != null )
            {
                url = defaultRegisteredService.LoginUrl;
            }
            if ( string.IsNullOrEmpty( url ) )
            {
                url = ConfigurationManager.AppSettings[ "DEFAULT_REDIRECT_URL" ];
            }

            return AppendCSTToken( url );
        }

        protected string GetServiceRedirectUrl()
        {
            string url = context.Request.QueryString[ "returnURL" ];
            if ( string.IsNullOrEmpty( url ) )
            {
                return null;
            }
            else
            {
                return AppendCSTToken( url );
            }
        }

        protected string AppendCSTToken( string url )
        {
            string cstToken = new ClientToServiceTicketDTO( cst ).GetToken( GetService().SecretKey );
            Log.Debug( "cstToken:" + cstToken );

            //TODO:
            //
            // if ( UrlContainsParam( url, "cst" ) )
            // {
            //     throw new ApplicationException( "无法登陆应用系统。" );
            // }

            if ( url.IndexOf( '?' ) < 0 )
            {
                url = string.Format( "{0}?cst={1}", url, HttpUtility.UrlEncode( cstToken ) );
            }
            else
            {
                //url = url.Replace("&", HttpUtility.UrlEncode("&"));
                url = string.Format( "{0}&cst={1}", url, HttpUtility.UrlEncode( cstToken ) );
            }
            Log.Debug( "redirect to " + url );

            // asp.net default max url length is 2048
            if ( url.Length > 2048 )
            {
                throw new ApplicationException( "redirectUrl length is too large." );
            }
            return url;
        }

        private bool UrlContainsParam( string url, string paramName )
        {
            var queryParams = HttpUtility.ParseQueryString( new Uri( url ).Query );
            return !string.IsNullOrEmpty( queryParams[ "cst" ] );
        }
    }
}