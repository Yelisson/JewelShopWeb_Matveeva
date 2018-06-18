<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormHangarsLoad.aspx.cs" Inherits="JewelShopWebView.FormHangarsLoad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Table ID="Table" runat="server">
        </asp:Table>
    
    </div>
        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Cохранить в excel" Width="117px" />
        <asp:Button ID="ButtonBack" runat="server" OnClick="ButtonBack_Click" Text="Вернуться" Width="115px" />
    </form>
</body>
</html>
