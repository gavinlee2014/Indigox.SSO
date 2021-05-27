using Indigox.Common.Logging;
using System;
using System.Web;

namespace Indigox.SSO.Application.Web
{
    public class ClearAuthHandler : IHttpHandler
    {
        protected HttpContext context;

        public bool IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            Log.Debug("ClearAuthHandler begin");
            this.context = context;
            if ((context.Request.Headers["Authorization"] != null) && 
                context.Request.Headers["Authorization"].StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                Log.Debug("ClearAuthHandler has Authorization");
                context.Response.StatusCode = 200;
                return;
            }
            Log.Debug("ClearAuthHandler has not Authorization");
            context.Response.AppendHeader("WWW-Authenticate", "basic realm=\"My Realm\"");
            //context.Response.AppendHeader("Connection", "close");
            context.Response.StatusCode = 401; // Unauthorized;
            context.Response.StatusDescription = "Unauthorized";
            context.Response.Clear();
        }
    }
}
