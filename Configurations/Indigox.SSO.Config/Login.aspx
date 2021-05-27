<%@ Page Language="C#" AutoEventWireup="true" Inherits="Indigox.SSO.Application.Web.DefaultLoginPage, Indigox.SSO.Application" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link rel="Stylesheet" href="Style/SSOLoginStyle.css" />
    <style type="text/css">
        #Info
        {
            color: Red;
            font-size: small;
        }
    </style>
    <script type="text/javascript">
        function focusUserNameInput() {
            var input = document.getElementById('UserName');
            input.select();
            input.focus();
        }
    </script>
</head>
<body onload="focusUserNameInput();">
    <div class="span7 well">
        <form id="form1" runat="server" class="form-horizontal">
        <div class="span6">
            <div class="control-group">
                <label class="control-label" for="UserName">
                    用户名</label>
                <div class="controls">
                    <asp:TextBox runat="server" CssClass="input-xlarge" ID="UserName"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="Password">
                    密码</label>
                <div class="controls">
                    <asp:TextBox runat="server" CssClass="input-xlarge" ID="Password" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="Login">
                </label>
                <div class="controls">
                    <asp:Button runat="server" ID="Login" class="btn btn-primary" Text="登 录" OnClick="Login_Click" />
                    <a class="btn btn-inverse" href="Logout.aspx">注销</a>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">
                </label>
                <div class="controls">
                    <asp:Label runat="server" ID="Info"></asp:Label>
                </div>
            </div>
        </div>
        </form>
    </div>
</body>
</html>