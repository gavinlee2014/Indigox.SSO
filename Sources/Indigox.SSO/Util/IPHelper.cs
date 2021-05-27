using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Indigox.SSO.Util
{
    public class IPHelper
    {
        /// <summary> 
        /// 取得客户端真实IP，如果有代理则取第一个非内网地址
        /// http://www.cnblogs.com/craig/archive/2008/11/18/1335809.html
        /// http://en.wikipedia.org/wiki/X-Forwarded-For
        /// </summary> 
        public static string GetIPAddress()
        {
            string proxy_ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];//取得代理IP
            string ip;
            if (string.IsNullOrEmpty(proxy_ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] ipArray = proxy_ip.Split(',');
                ip = ipArray[0];
            }
            return ip;
        }

    }
}
