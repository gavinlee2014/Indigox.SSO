using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Indigox.Common.Logging;
using Indigox.Common.Utilities;

namespace Indigox.SSO.Application.Util
{
    public static class TicketSerializer
    {
        public static string Serialize<T>( T ticket, string secretKey )
        {
            if ( ticket == null )
            {
                return null;
            }

            Type type = typeof( T );
            PropertyInfo[] properties = type.GetProperties();
            string[] segments = new string[ properties.Length ];

            for ( int i = 0; i < properties.Length; i++ )
            {
                PropertyInfo property = properties[ i ];
                object value = property.GetValue( ticket, null );
                if ( value is Array )
                {
                    List<string> valueSegments = new List<string>();
                    foreach ( object item in (Array)value )
                    {
                        valueSegments.Add( GetSafeValue( Convert.ToString( item ) ) );
                    }
                    segments[ i ] = string.Format( "{0}={1}", property.Name, string.Join( ",", valueSegments.ToArray() ) );
                }
                else
                {
                    segments[ i ] = string.Format( "{0}={1}", property.Name, GetSafeValue( Convert.ToString( value ) ) );
                }
            }

            string plainText = string.Join( ";", segments );
            //Console.WriteLine( "plainText:" + plainText );

            string token = DESCrypt.Encrypt( plainText, secretKey );
            //Console.WriteLine( "token:" + token );

            return token;
        }

        public static T Deserialize<T>( string token, string secretKey )
        {
            if ( string.IsNullOrEmpty( token ) )
            {
                return default( T ); // null
            }

            try
            {
                Type type = typeof( T );

                string plainText = DESCrypt.Dncrypt( token, secretKey );
                string[] segments = plainText.Split( new string[] { ";" }, StringSplitOptions.None );

                T ticket = (T)Activator.CreateInstance( type );
                foreach ( string segment in segments )
                {
                    int index = segment.IndexOf( '=' );
                    string name = segment.Substring( 0, index );
                    string value = segment.Substring( index + 1 );

                    PropertyInfo property = type.GetProperty( name );
                    Type propertyType = property.PropertyType;

                    if ( propertyType.IsArray )
                    {
                        Type elementType = propertyType.GetElementType();
                        string[] valueSegments = value.Split( new string[] { "," }, StringSplitOptions.None );
                        Array array = Array.CreateInstance( elementType, valueSegments.Length );
                        for ( int i = 0; i < array.Length; i++ )
                        {
                            array.SetValue( Convert.ChangeType( ParseSafeValue( valueSegments[ i ] ), elementType ), i );
                        }
                        property.SetValue( ticket, array, null );
                    }
                    else
                    {
                        property.SetValue( ticket, Convert.ChangeType( ParseSafeValue( value ), propertyType ), null );
                    }
                }

                return ticket;
            }
            catch ( Exception ex )
            {
                Log.Error( string.Format( "Deserialize token failed. {1}\r\n\tToken: {0}", token, ex.Message ) );
                return default( T ); // null
            }
        }

        private static string GetSafeValue( string value )
        {
            return HttpUtility.UrlEncode( value );
        }

        private static string ParseSafeValue( string safeValue )
        {
            return HttpUtility.UrlDecode( safeValue );
        }
    }
}