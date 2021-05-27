using System;
using System.Configuration;
using System.Web;
using Indigox.Common.Logging;
using Indigox.SSO.Application.DTO;
using Indigox.SSO.Interface;
using Indigox.SSO.Services;

namespace Indigox.SSO.Application.Web
{
    public class BaseLoginPage : System.Web.UI.Page
    {
        protected IRegisteredService Service
        {
            get
            {
                string serviceID = Request.QueryString[ "service" ];

                IRegisteredService service;

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
        }

        protected string GetDefaultRedirectUrl()
        {
            string redirectUrl = null;
            ServiceManager serviceManager = new ServiceManager();
            RegisteredService defaultRegisteredService = serviceManager.GetDefaultRegisteredService();
            if ( defaultRegisteredService != null )
            {
                redirectUrl = defaultRegisteredService.LoginUrl;
            }
            if ( string.IsNullOrEmpty( redirectUrl ) )
            {
                redirectUrl = ConfigurationManager.AppSettings[ "DEFAULT_REDIRECT_URL" ];
            }
            return redirectUrl;
        }

        protected string GetRedirectToServiceUrl( IClientToServiceTicket cst )
        {
            string cstToken = new ClientToServiceTicketDTO( cst ).GetToken( Service.SecretKey );
            Log.Debug( "cstToken:" + cstToken );

            string redirectUrl = Request.QueryString[ "returnURL" ];
            if ( string.IsNullOrEmpty( redirectUrl ) )
            {
                redirectUrl = GetDefaultRedirectUrl();
            }

            if ( redirectUrl.IndexOf( '?' ) < 0 )
            {
                redirectUrl = string.Format( "{0}?cst={1}", redirectUrl, HttpUtility.UrlEncode( cstToken ) );
            }
            else
            {
                Log.Debug("baselogin");
                //redirectUrl = redirectUrl.Replace("&",HttpUtility.UrlEncode("&"));
                redirectUrl = string.Format( "{0}&cst={1}", redirectUrl, HttpUtility.UrlEncode( cstToken ) );
            }
            Log.Debug( "redirect to " + redirectUrl );

            // asp.net default max url length is 2048
            if ( redirectUrl.Length > 2048 )
            {
                throw new ApplicationException( "redirectUrl length is too large." );
            }

            return redirectUrl;
        }

        /// <summary>
        /// 获取用户是否显示的请求访问登录页面，如果是，即使有 TicketGrantTicket 也会继续停留在登录页。
        /// </summary>
        protected bool IsExplicitLoginRequest()
        {
            return ( string.IsNullOrEmpty( Request.QueryString[ "returnURL" ] ) );
        }
    }
}