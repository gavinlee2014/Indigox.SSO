using Indigox.Common.Logging;
using Indigox.Common.Serialization;
using Indigox.Common.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Indigox.SSO.Application.Web
{
    public class WeChatEntAuthenticationHandler : AbstractAuthenticationHandler
    {
        private WeChatEntUserData userData;
        private const string WECHAT_ENT_AUTH_URL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";
        private const string WECHAT_ENT_USER_INFO_API = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
        private const string WECHAT_ENT_ACCESS_TOKEN_API = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        public override void ProcessRequest(HttpContext context)
        {
            if (!context.Request.Params.AllKeys.Contains<string>("code"))
            {
                string url = String.Format(WECHAT_ENT_AUTH_URL, Settings.Instance.WeChatEntAuthUserName, System.Uri.EscapeDataString(context.Request.Url.AbsoluteUri));
                Log.Debug("WeChatEntAuthenticationHandler redirect to " + url);
                context.Response.Redirect(url);
                return;
            }
            string code = context.Request.Params["code"];
            using (WebClient client = new WebClient())
            {
                //string authString = EncodeUtil.Base64Encode(String.Format("{0}:{1}", Settings.Instance.DingAuthUserName, Settings.Instance.DingAuthUserPwd), Encoding.UTF8);
                //client.Headers.Add("Authorization", "Basic " + authString);
                string url = String.Format(WECHAT_ENT_USER_INFO_API, GetAccessToken(), code);
                Log.Debug("query user info url:" + url);
                string ret = Encoding.UTF8.GetString(client.DownloadData(url));
                Log.Debug("query user info result:" + ret);
                JsonSerializer serializer = new JsonSerializer();
                WeChatEntUserData result = serializer.Deserialize<WeChatEntUserData>(ret);
                userData = result;
            }
            base.ProcessRequest(context);
        }

        protected override string GetUserName()
        {
            return userData.UserId;
        }

        private string GetAccessToken()
        {
            using (WebClient client = new WebClient())
            {
                //string authString = EncodeUtil.Base64Encode(String.Format("{0}:{1}", Settings.Instance.DingAuthUserName, Settings.Instance.DingAuthUserPwd), Encoding.UTF8);
                //client.Headers.Add("Authorization", "Basic " + authString);
                string url = String.Format(WECHAT_ENT_ACCESS_TOKEN_API, Settings.Instance.WeChatEntAuthUserName, Settings.Instance.WeChatEntAuthUserPwd);
                Log.Debug("query access token url:" + url);
                string ret = Encoding.UTF8.GetString(client.DownloadData(url));
                Log.Debug("query access token result:" + ret);
                JsonSerializer serializer = new JsonSerializer();
                WeChatEntAccessTokenData result = serializer.Deserialize<WeChatEntAccessTokenData>(ret);
                return result.access_token;
            }
        }

        private class WeChatEntAccessTokenData
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        private class WeChatEntUserData
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string UserId { get; set; }
            public string DeviceId { get; set; }
        }
    }
}
