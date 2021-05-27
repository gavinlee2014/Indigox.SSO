using System;
using System.Web;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    internal class AccessTicketStorage
    {
        private HttpContext context;
        private CookieHelper cookieHelper;

        public AccessTicketStorage( HttpContext context )
        {
            this.context = context;
            this.cookieHelper = new CookieHelper( context );
        }

        public void Save( AccessTicket accessTicket )
        {
            string accessTicketToken = accessTicket.GetToken();
            Log.Debug( "Save access ticket...\r\n\ttoken:" + accessTicketToken );
            cookieHelper.Set( Settings.Instance.AccessTicketCookieName, accessTicketToken );
        }

        public AccessTicket Load()
        {
            Log.Debug( "Load access ticket..." );

            string accessTicketToken = cookieHelper.Get( Settings.Instance.AccessTicketCookieName );
            if ( string.IsNullOrEmpty( accessTicketToken ) )
            {
                Log.Debug( "No access ticket cookie." );
                return null;
            }

            AccessTicket accessTicket = AccessTicket.GetFrom( accessTicketToken );
            if ( accessTicket == null )
            {
                Log.Debug( "Can't get access ticket from token: " + accessTicketToken );
                return null;
            }

            return accessTicket;
        }

        public void Remove()
        {
            Log.Debug( "Remove access ticket..." );
            cookieHelper.Remove( Settings.Instance.AccessTicketCookieName );
        }
    }
}