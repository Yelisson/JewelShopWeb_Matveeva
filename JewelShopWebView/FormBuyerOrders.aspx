<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormBuyerOrders.aspx.cs" Inherits="JewelShopWebView.FormBuyerOrders" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        C<asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        По<asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
        <asp:Button ID="Button1" runat="server" Text="Button" Width="120px" />
        <asp:Button ID="Button2" runat="server" Text="Button" Width="113px" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    </div>
    </form>
</body>
</html>
