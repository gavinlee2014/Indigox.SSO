using System;
using System.Configuration;

namespace Indigox.SSO
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

        /// <summary>
        /// Validate Handler处理之后，跳转到的login handler或者aspx的路径，例如/WindowsAuthenticationHandler.ashx
        /// </summary>
        public string LoginUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["LOGIN_URL"];
                return url;
            }
        }

        public string DingLoginUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["DING_LOGIN_URL"];
                return url;
            }
        }

        public string DingAuthUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["DING_AUTH_URL"];
                return url;
            }
        }

        public string DingUserInfoAPI
        {
            get
            {
                string url = ConfigurationManager.AppSettings["DING_USER_INFO_API"];
                return url;
            }
        }

        public string DingAuthUserName
        {
            get
            {
                string url = ConfigurationManager.AppSettings["DING_AUTH_USERNAME"];
                return url;
            }
        }

        public string DingAuthUserPwd
        {
            get
            {
                string url = ConfigurationManager.AppSettings["DING_AUTH_USERPWD"];
                return url;
            }
        }

        public string FederatedLoginUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["FEDERATED_LOGIN_URL"];
                return url;
            }
        }

        public string FormLoginUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["FORM_LOGIN_URL"];
                return url;
            }
        }

        public string SecretKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings[ "SECRET_KEY" ];
                AssertKey( key );
                return key;
            }
        }

        public string TicketGrantingTicketCookieName
        {
            get
            {
                return ConfigurationManager.AppSettings[ "TGT_COOKIE_NAME" ];
            }
        }

        private void AssertKey( string key )
        {
            if ( key.Length != 8 )
            {
                throw new ApplicationException( "密钥长度要求8位长度。" );
            }
        }

        public bool NotValidatePassword
        {
            get
            {
                string value = ConfigurationManager.AppSettings[ "NOT_VALIDATE_PASSWORD" ];
                if ( string.IsNullOrEmpty( value ) )
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean( value );
                }
            }
        }

        public string WindowsAuthenticationSecertKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings[ "WINDOWS_AUTHENTICATION_SECERT_KEY" ];
                AssertKey( key );
                return key;
            }
        }
    }
}