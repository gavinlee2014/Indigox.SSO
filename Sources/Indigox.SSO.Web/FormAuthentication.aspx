<%@ Page Language="C#" %>
<%@ Import Namespace="Indigox.Common.Logging" %>
<%@ Import Namespace="Indigox.SSO.Application.DTO" %>
<%@ Import Namespace="Indigox.SSO.Application.Web" %>
<%@ Import Namespace="Indigox.SSO.Authentication" %>
<%@ Import Namespace="Indigox.SSO.Interface" %>
<%@ Import Namespace="Indigox.SSO.Services" %>
<script runat="server">

    Indigox.Web.SSO.LastLoginInfoStorage LastLoginInfoStorage = new Indigox.Web.SSO.LastLoginInfoStorage();
    Indigox.Web.SSO.EACAuthentication EACAuthentication = new Indigox.Web.SSO.EACAuthentication();
    Indigox.Web.SSO.LoginHistory LoginHistory = new Indigox.Web.SSO.LoginHistory();
    Indigox.Web.SSO.UserNameValidator UserNameValidator = new Indigox.Web.SSO.UserNameValidator();

    Indigox.SSO.Interface.IRegisteredService service;

    Indigox.SSO.Interface.ITicketGrantingTicket tgt;

    Indigox.SSO.Interface.IClientToServiceTicket cst;

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !base.IsPostBack )
        {
            EACAuthentication.Authenticate();
            if ( EACAuthentication.IsPostBack() && EACAuthentication.ValidateAuthToken() )
            {
                LoginHistory.ResponseCookies();
                EACAuthentication.RedirectFromLoginPage();
            }
        }
        if ( !base.IsPostBack )
        {
            this.txtUsername.Text = LastLoginInfoStorage.LoadUsernameCookie();
        }
    }

    protected void btnLogin_Click( object sender, EventArgs e )
    {
        LastLoginInfoStorage.SaveUsernameCookie( this.txtUsername.Text );
        Login( this.txtUsername.Text, this.txtPassword.Text );
    }

    protected string GetUserName()
    {
        return this.txtUsername.Text;
    }

    public void Login( string username, string password )
    {
        if ( UserNameValidator.Validate( username ) )
        {
            //if ( password != "111" )
            //{
            //    lblErrorInfo.Text = "密码不正确。";
            //    lblErrorInfo.Visible = true;
            //}

            // FormsAuthentication.SetAuthCookie( username, false );
            // FormsAuthentication.RedirectFromLoginPage( username, false );
            if (!Indigox.Common.ADAccessor.Accessor.CheckPassword(username, password)) {
                lblErrorInfo.Text = "密码不正确。";
                lblErrorInfo.Visible = true;
            }
            else {
                lblErrorInfo.Text = "";
                lblErrorInfo.Visible = false;

                CreateTicketGrantingTicket();
                GrantClientToServiceTicket();
                SendClientToServiceTicket();
            }
        }
        else
        {
            lblErrorInfo.Text = "不存在此用户。";
            lblErrorInfo.Visible = true;
        }
    }

    protected void CreateTicketGrantingTicket()
    {
        Indigox.SSO.Interface.IAuthentication authentication = new Indigox.SSO.Authentication.Authentication( new Principal( GetUserName() ) );
        tgt = Indigox.SSO.CentralAuthenticationService.Instance.CreateTicketGrantingTicket( authentication );

        TicketGrantingTicketStorage storage = new TicketGrantingTicketStorage( Context );
        storage.Save( tgt );
    }

    protected void GrantClientToServiceTicket()
    {
        cst = Indigox.SSO.CentralAuthenticationService.Instance.GrantClientToServiceTicket( tgt, GetService() );
        TicketGrantingTicketStorage storage = new TicketGrantingTicketStorage( Context );
        storage.Save( tgt );
    }

    protected void SendClientToServiceTicket()
    {
        string url = GetServiceRedirectUrl();
        if ( url == null )
        {
            url = GetDefaultRedirectUrl();
        }
        Response.Redirect(url, false); 
    }

    protected IRegisteredService GetService()
    {
        string serviceID = Request.QueryString[ "service" ];

        ServiceManager serviceManager = new ServiceManager();
        if ( string.IsNullOrEmpty( serviceID ) )
        {
            service = serviceManager.GetDefaultRegisteredService();
        }
        else
        {
            service = serviceManager.GetRegisteredService( serviceID );
        }

        return service;
    }

    protected string GetDefaultRedirectUrl()
    {
        string url = null;
        ServiceManager serviceManager = new ServiceManager();
        RegisteredService defaultRegisteredService = serviceManager.GetDefaultRegisteredService();
        if ( defaultRegisteredService != null )
        {
            url = defaultRegisteredService.LoginUrl;
        }
        if ( string.IsNullOrEmpty( url ) )
        {
            url = ConfigurationManager.AppSettings[ "DEFAULT_REDIRECT_URL" ];
        }

        return AppendCSTToken( url );
    }

    protected string GetServiceRedirectUrl()
    {
        string url = Request.QueryString[ "returnURL" ];
        if ( string.IsNullOrEmpty( url ) )
        {
            return null;
        }
        else
        {
            return AppendCSTToken( url );
        }
    }

    protected string AppendCSTToken( string url )
    {
        string cstToken = new ClientToServiceTicketDTO( cst ).GetToken( GetService().SecretKey );
        Log.Debug( "cstToken:" + cstToken );

        //TODO:
        //
        // if ( UrlContainsParam( url, "cst" ) )
        // {
        //     throw new ApplicationException( "无法登陆应用系统。" );
        // }

        if ( url.IndexOf( '?' ) < 0 )
        {
            url = string.Format( "{0}?cst={1}", url, HttpUtility.UrlEncode( cstToken ) );
        }
        else
        {
            //url = url.Replace("&", HttpUtility.UrlEncode("&"));
            url = string.Format( "{0}&cst={1}", url, HttpUtility.UrlEncode( cstToken ) );
        }
        Log.Debug( "redirect to " + url );

        // asp.net default max url length is 2048
        if ( url.Length > 2048 )
        {
            throw new ApplicationException( "redirectUrl length is too large." );
        }
        return url;
    }

</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=yes" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>登录</title>
    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }
        html, body {
            width: 100%;
            height: 100%;
            background: #fff;
            text-align: left;
            font: 13px/18px Arial;
        }
        .pos { width: 100%; height: 50%; visibility: hidden; }
        .login {
            width: 320px;
            margin: -200px auto auto auto;;
        }
        .login h2 {
            font-size: 16px;
            line-height: 17px;
            height: 16px;
            margin: 0 0 1.2em;
            font-weight: normal;
        }
        .logo-m{
            float: right;
            height: 34px;
            width: 235px;
            float: right;
            background-image: url(img/logo-m.png);
            background-repeat: no-repeat;
            background-position: right top;
        }
        .login form {
            background: #F1F1F1;
            border: 1px solid #E5E5E5;
            padding: 20px 25px 15px;
        }
        .login form p {
            margin: 0 0 1.5em;
            overflow: visible;
        }
        .login form p label {
            color: #222;
            font-weight: bold;
            margin: 0px 0px 0.5em;
            display: block;
        }
        .login form p .ipt_txt {
            border: 1px solid #D9D9D9;
            border-top: 1px solid #C0C0C0;
            width: 250px;
            height: 32px;
            font-size: 15px;
            direction: ltr;
            margin: 0;
            padding: 0 8px;
        }
        .login form p .ipt_txt:hover {
            border: 1px solid #B9B9B9;
            border-top: 1px solid #A0A0A0;
            box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.1);
        }
        .login form .ipt_btn {
            margin: 0;
            padding: 0 8px;
            min-width: 46px;
            height: 32px;
            font-size: 13px;
            font-weight: bold;
            font-family: sans-serif;
            line-height: 29px;
            color: #fff;
            text-shadow: 0 1px rgba(0, 0, 0, 0.1);
            vertical-align: bottom;
            border: 1px solid #3079ED;
            border-radius: 2px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            background-color: #4D90FE;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#4D90FE),to(#4787ED));
            background-image: -webkit-linear-gradient(top,#4D90FE,#4787ED);
            background-image: -moz-linear-gradient(top,#4D90FE,#4787ED);
            background-image: -ms-linear-gradient(top,#4D90FE,#4787ED);
            background-image: -o-linear-gradient(top,#4D90FE,#4787ED);
            background-image: linear-gradient(top,#4D90FE,#4787ED);
            text-align: center;
            overflow: hidden;
            cursor: pointer;
        }
        .login form .ipt_btn:hover
        {
            border: 1px solid #2F5BB7;
            background-color: #357AE8;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#4D90FE),to(#357AE8));
            background-image: -webkit-linear-gradient(top,#4D90FE,#357AE8);
            background-image: -moz-linear-gradient(top,#4D90FE,#357AE8);
            background-image: -ms-linear-gradient(top,#4D90FE,#357AE8);
            background-image: -o-linear-gradient(top,#4D90FE,#357AE8);
            background-image: linear-gradient(top,#4D90FE,#357AE8);
        }
        .login .ft {
            text-align: center;
            width: auto;
            height: 37px;
            color: #737473;
            padding-top: 14px;
        }
        .login .ft .en {
            font-size: 10px;
            font-weight: bold;
        }
        .err {
            color:#DD4B39 !important;
        }
    </style>
    <style type="text/css" media="(max-height: 480px)">
        .pos {
            display: none;
        }
        .login {
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <div class="pos">
    </div>
    <div class="login">
        <form id="form1" runat="server">
        <h2>登录<strong class="logo-m"></strong></h2>
        <p>
            <label>用户名：</label><asp:TextBox ID="txtUsername" class="ipt_txt" runat="server"></asp:TextBox>
        </p>
        <p>
            <label>密 码：</label><asp:TextBox ID="txtPassword" class="ipt_txt" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <div>
            <asp:Label ID="lblErrorInfo" CssClass="err" runat="server" Visible="false"></asp:Label>
        </div>
        <p>
            <asp:Button ID="btnLogin" CssClass="ipt_btn" runat="server" OnClick="btnLogin_Click" Text="登录" />
        </p>
        </form>
    </div>
</body>
</html>
