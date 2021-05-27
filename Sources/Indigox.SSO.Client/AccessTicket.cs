using System;
using Indigox.SSO.Client.Util;

namespace Indigox.SSO.Client
{
    public class AccessTicket
    {
        public AccessTicket()
        {
        }

        public AccessTicket( string id, string userName )
        {
            if ( string.IsNullOrEmpty( id ) )
            {
                throw new ArgumentNullException( "id" );
            }

            if ( string.IsNullOrEmpty( userName ) )
            {
                throw new ArgumentNullException( "userName" );
            }

            this.ID = id;
            this.UserName = userName;
        }

        public DateTime CreateTime { get; set; }

        public string ID { get; set; }

        public string UserName { get; set; }

        public static AccessTicket GetFrom( string token )
        {
            return TicketSerializer.Deserialize<AccessTicket>( token, Settings.Instance.SecretKey );
        }

        public string GetToken()
        {
            return TicketSerializer.Serialize<AccessTicket>( this, Settings.Instance.SecretKey );
        }
    }
}