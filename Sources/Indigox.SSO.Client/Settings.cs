using System;
using System.Configuration;

namespace Indigox.SSO.Client
{
    public class Settings
    {
        private static Settings instance = new Settings();

        private Settings()
        {
        }

        public static Settings Instance
        {
            get { return instance; }
        }

        public string SSOLoginUrl
        {
            get { return ConfigurationManager.AppSettings[ "SSO_LOGIN_URL" ]; }
        }

        public string SSOLogoutUrl
        {
            get { return ConfigurationManager.AppSettings[ "SSO_LOGOUT_URL" ]; }
        }

        public string ServiceID
        {
            get { return ConfigurationManager.AppSettings[ "SERVICE_ID" ]; }
        }

        public string SecretKey
        {
            get { return ConfigurationManager.AppSettings[ "SECRET_KEY" ]; }
        }

        public string SSOTicketServiceUrl
        {
            get { return ConfigurationManager.AppSettings[ "SSO_TICKET_SERVICE_URL" ]; }
        }

        public string AccessTicketCookieName
        {
            get { return ConfigurationManager.AppSettings[ "ACCESS_TICKET_COOKIE" ]; }
        }

        /// <remarks>
        /// 多个值以分号(;)分隔
        /// </remarks>
        public string AuthCookies
        {
            get { return ConfigurationManager.AppSettings[ "AUTH_COOKIES" ]; }
        }

        /// <remarks>
        /// 多个值以分号(;)分隔
        /// </remarks>
        public string SSOModuleIgnorePath
        {
            get { return ConfigurationManager.AppSettings[ "SSO_MODULE_IGNORE_PATH" ]; }
        }

        /// <remarks>
        /// Default is /sso/login?ReturnUrl={ReturnUrl}
        /// </remarks>
        public string LoginUrl
        {
            get
            {
                string value = ConfigurationManager.AppSettings[ "LOGIN_URL" ];
                if ( string.IsNullOrEmpty( value ) )
                {
                    return "/sso/login?ReturnUrl={ReturnUrl}";
                }
                else
                {
                    return value;
                }
            }
        }
    }
}