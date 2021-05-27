using Indigox.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Indigox.SSO.Application.Web
{
    public class DingTalkCallbackHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable => false;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            Log.Debug("ding callback " + context.Request.QueryString);
            string accessToken = context.Request.Params["code"];
            //IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getuserinfo");
            //OapiUserGetuserinfoRequest req = new OapiUserGetuserinfoRequest();
            //req.SetHttpMethod("GET");
            //OapiUserGetuserinfoResponse res = client.Execute(req, accessToken);
            //Log.Debug("ding response:" + res.Body);
        }
    }
}
