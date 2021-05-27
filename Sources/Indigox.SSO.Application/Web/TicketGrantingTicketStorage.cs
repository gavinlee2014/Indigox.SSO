using System;
using System.Web;
using Indigox.Common.Logging;
using Indigox.SSO.Application.DTO;
using Indigox.SSO.Application.Util;
using Indigox.SSO.Interface;

namespace Indigox.SSO.Application.Web
{
    public class TicketGrantingTicketStorage
    {
        private HttpContext context;
        private CookieHelper cookieHelper;
        private string secretKey;
        private string cookieName;

        public TicketGrantingTicketStorage( HttpContext context )
        {
            this.context = context;
            this.cookieHelper = new CookieHelper( context );
            this.secretKey = Settings.Instance.SecretKey;
            this.cookieName = Settings.Instance.TicketGrantingTicketCookieName;
        }

        public void Save( ITicketGrantingTicket ticket )
        {
            string token = new TicketGrantingTicketDTO( ticket ).GetToken( secretKey );
            Log.Debug( "Save tgt...\r\n\ttoken:" + token );
            cookieHelper.Set( Settings.Instance.TicketGrantingTicketCookieName, token );
        }

        public ITicketGrantingTicket Load()
        {
            Log.Debug( "Load tgt..." );

            string token = cookieHelper.Get( cookieName );
            if ( string.IsNullOrEmpty( token ) )
            {
                Log.Debug( "No tgt cookie." );
                return null;
            }

            ITicketGrantingTicket ticket = TicketGrantingTicketDTO.GetTicket( token, secretKey );
            if ( ticket == null )
            {
                Log.Debug( "Can't get tgt from token: " + token );
                return null;
            }

            return ticket;
        }

        public void Remove()
        {
            Log.Debug( "Remove tgt cookie..." );
            cookieHelper.Remove( cookieName );
        }
    }
}