using System;
using System.Web;

namespace Indigox.SSO.Client.Util
{
    public class CookieHelper
    {
        private HttpContext context;

        public CookieHelper( HttpContext context )
        {
            this.context = context;
        }

        public void Set( string key, string value )
        {
            HttpCookie cookie = new HttpCookie( key );
            cookie.Value = value;
            context.Response.Cookies.Set( cookie );
        }

        public string Get( string key )
        {
            HttpCookie cookie = context.Request.Cookies[ key ];
            if ( cookie == null )
            {
                return null;
            }
            else
            {
                return cookie.Value;
            }
        }

        public void Remove( string key )
        {
            HttpCookie cookie = context.Request.Cookies[ key ];
            if ( cookie != null )
            {
                cookie.Expires = DateTime.Now.AddDays( -1 );
                context.Response.Cookies.Set( cookie );
            }
        }

        public void Clear()
        {
            HttpCookieCollection cookies=context.Request.Cookies;
            foreach (string key in cookies.AllKeys)
            {
                Remove(key);
            }
        }
    }
}