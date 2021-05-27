using System;
using System.Security.Cryptography;
using System.Text;

namespace Indigox.SSO.Client.Util
{
    public class DESCrypt
    {
        public static string Encrypt( String plainText, string key )
        {
            using ( DESCryptoServiceProvider des = new DESCryptoServiceProvider() )
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes( plainText );
                des.Key = ASCIIEncoding.ASCII.GetBytes( key );
                des.IV = ASCIIEncoding.ASCII.GetBytes( key );
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using ( CryptoStream cs = new CryptoStream( ms, des.CreateEncryptor(), CryptoStreamMode.Write ) )
                {
                    cs.Write( inputByteArray, 0, inputByteArray.Length );
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String( ms.ToArray() );
                ms.Close();
                return str;
            }
        }

        public static string Decrypt( String cipherText, string key )
        {
            byte[] inputByteArray = Convert.FromBase64String( cipherText );
            using ( DESCryptoServiceProvider des = new DESCryptoServiceProvider() )
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes( key );
                des.IV = ASCIIEncoding.ASCII.GetBytes( key );
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using ( CryptoStream cs = new CryptoStream( ms, des.CreateDecryptor(), CryptoStreamMode.Write ) )
                {
                    cs.Write( inputByteArray, 0, inputByteArray.Length );
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString( ms.ToArray() );
                ms.Close();
                return str;
            }
        }
    }
}