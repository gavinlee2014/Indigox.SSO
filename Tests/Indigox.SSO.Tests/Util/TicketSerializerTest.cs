using System;
using Indigox.Common.Serialization;
using Indigox.SSO.Application.Util;
using NUnit.Framework;

namespace Indigox.SSO.Tests.Util
{
    public class TicketSerializerTest
    {
        private const string SecertKey = "12345678";

        private static JsonSerializer serializer = new JsonSerializer();

        [Test]
        public void TestGetFrom()
        {
            string token = "J0zAmziia71/00MJvrDvLBoTUtBGAWb95jWS8PFggEaUQDJuhBqqiLj3fuxgWruAvCXbJjU0XkkGa60Ax6b7+6mgl90d+D2S5QU15VRRBM61JdbtQk+yA5X8k0W/iCfKDAsvIMxe/phZrbXkK2CwCZe23YfH8N2p";
            Ticket ticket = TicketSerializer.Deserialize<Ticket>( token, SecertKey );
            string actual = serializer.Serialize( ticket );
            string expected = @"{""ID"":""tgt123"",""IPAddress"":""192.168.0.1"",""UserName"":""user1#,;"",""CreateTime"":""\/Date(1375424529000+0800)\/"",""ServiceIDs"":[""123"",""332"",""442""]}";
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void TestGetToken()
        {
            Ticket ticket = new Ticket( "tgt123" )
            {
                IPAddress = "192.168.0.1",
                ServiceIDs = new string[] { "123", "332", "442" },
                UserName = "user1#,;",
                CreateTime = new DateTime( 2013, 8, 2, 14, 22, 9 )
            };
            string actual = TicketSerializer.Serialize<Ticket>( ticket, SecertKey );
            string expected = "J0zAmziia71/00MJvrDvLBoTUtBGAWb95jWS8PFggEaUQDJuhBqqiLj3fuxgWruAvCXbJjU0XkkGa60Ax6b7+6mgl90d+D2S5QU15VRRBM61JdbtQk+yA5X8k0W/iCfKDAsvIMxe/phZrbXkK2CwCZe23YfH8N2p";
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void TestSelfValidate()
        {
            Ticket ticket = new Ticket( "tgt123" )
            {
                IPAddress = "192.168.0.1",
                ServiceIDs = new string[] { "123", "332", "442" },
                UserName = "user1#,;",
                CreateTime = new DateTime( 2013, 8, 2, 14, 22, 9 )
            };
            string ticketJson = serializer.Serialize( ticket );
            Console.WriteLine( ticketJson );

            string token = TicketSerializer.Serialize<Ticket>( ticket, SecertKey );
            Console.WriteLine( token );

            Ticket newTicket = TicketSerializer.Deserialize<Ticket>( token, SecertKey );
            string newTicketJson = serializer.Serialize( newTicket );
            Assert.AreEqual( ticketJson, newTicketJson );
        }

        private class Ticket
        {
            public Ticket()
            {
                // TicketSerializer need an public no argument constructor.
            }

            public Ticket( string id )
            {
                this.id = id;
            }

            private string id;
            private string ipAddress;
            private string userName;
            private DateTime createTime;
            private string[] serviceIDs;

            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            public string IPAddress
            {
                get { return ipAddress; }
                set { ipAddress = value; }
            }
            public string UserName
            {
                get { return userName; }
                set { userName = value; }
            }
            public DateTime CreateTime
            {
                get { return createTime; }
                set { createTime = value; }
            }
            public string[] ServiceIDs
            {
                get { return serviceIDs; }
                set { serviceIDs = value; }
            }
        }
    }
}