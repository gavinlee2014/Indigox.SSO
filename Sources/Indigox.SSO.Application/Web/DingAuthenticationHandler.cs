using Indigox.Common.Serialization;
using Indigox.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Indigox.SSO.Application.Web
{
    public class DingAuthenticationHandler : AbstractAuthenticationHandler
    {
        private DingUserInfoData userData;

        public override void ProcessRequest(HttpContext context)
        {
            if (!context.Request.Params.AllKeys.Contains<string>("ldingCode"))
            {
                string url = String.Format("{0}{1}", Settings.Instance.DingAuthUrl, System.Uri.EscapeDataString(context.Request.Url.AbsoluteUri));
                context.Response.Redirect(url);
                return;
            }
            string dingCode = context.Request.Params["ldingCode"];
            using (WebClient client = new WebClient())
            {
                string authString = EncodeUtil.Base64Encode(String.Format("{0}:{1}",Settings.Instance.DingAuthUserName,Settings.Instance.DingAuthUserPwd), Encoding.UTF8);
                client.Headers.Add("Authorization", "Basic "+ authString);
                string url = String.Format("{0}{1}", Settings.Instance.DingUserInfoAPI, dingCode);
                string ret = Encoding.UTF8.GetString(client.DownloadData(url));
                JsonSerializer serializer = new JsonSerializer();
                DingUserInfoResult result = serializer.Deserialize<DingUserInfoResult>(ret);
                userData = result.Data;
            }
            base.ProcessRequest(context);
        }


        protected override string GetUserName()
        {
            if (this.userData != null)
            {
                return userData.LoginName;
            }
            else
            {
                throw new UnauthorizedAccessException("DingTalk not authorized");
            }
        }

        private class DingUserInfoResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public string Code { get; set; }
            public DingUserInfoData Data { get; set; }
        }

        private class DingUserInfoData
        {
            public string No { get; set; }
            public string UserId { get; set; }
            public string MobileNo { get; set; }
            public string LoginName { get; set; }
        }
    }
}
