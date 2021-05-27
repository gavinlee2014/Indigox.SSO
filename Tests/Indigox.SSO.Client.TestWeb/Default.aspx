<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Indigox.SSO.Client.TestWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Hello, <%= User.Identity.Name %>!
    </div>
    <div>
        <asp:Button ID="btnClearAuthCookie" runat="server" Text="Clear Auth Cookie" OnClick="btnClearAuthCookie_clicked" />
        <asp:Button ID="btnClearAccessTicket" runat="server" Text="Clear Access Ticket" OnClick="btnClearAccessTicket_clicked" />
    </div>
    </form>
</body>
</html>