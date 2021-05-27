using System;
using Indigox.Common.ADAccessor;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Authentication
{
    public class ADAuthenticator : IAuthenticator
    {
        public IAuthentication Authenticate( ICredentials credentials )
        {
            UsernamePasswordCredentials upc = credentials as UsernamePasswordCredentials;

            if ( upc == null )
            {
                throw new ApplicationException( "Can't convert to UsernamePasswordCredentials." );
            }

            AssertValidateUserName( upc.UserName );

            AssertValidatePassword( upc.UserName, upc.Password );

            Authentication authentication = new Authentication( new Principal( upc.UserName ) );
            return authentication;
        }

        private void AssertValidateUserName( string username )
        {
            if ( !Accessor.IsUserExist( username ) )
            {
                throw new ApplicationException( "用户不存在。" );
            }
        }

        private void AssertValidatePassword( string username, string password )
        {
            if ( !Settings.Instance.NotValidatePassword )
            {
                if ( !Accessor.CheckPassword( username, password ) )
                {
                    throw new ApplicationException( "密码错误。" );
                }
            }
        }
    }
}