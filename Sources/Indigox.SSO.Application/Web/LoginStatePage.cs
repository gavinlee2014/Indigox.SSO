using System;

namespace Indigox.SSO.Application.Web
{
    public class LoginStatePage : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            if ( !IsPostBack )
            {
                string state = Request.QueryString[ "state" ];
                int result = 0;
                if ( state == "1" )
                {
                    result = 1;
                }
                string script = "parent.callback(" + result + ");";
                Page.ClientScript.RegisterStartupScript( GetType(), "Script", script, true );
            }
        }
    }
}