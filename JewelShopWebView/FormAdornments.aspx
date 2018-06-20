<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormAdornments.aspx.cs" Inherits="JewelShopWebView.FormAdornments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="ButtonAdd" runat="server" Text="Добавить" OnClick="ButtonAdd_Click" />
        <asp:Button ID="ButtonChange" runat="server" Text="Изменить" OnClick="ButtonChange_Click" />
        <asp:Button ID="ButtonDelete" runat="server" Text="Удалить" OnClick="ButtonDelete_Click" />
        <asp:Button ID="ButtonUpd" runat="server" Text="Обновить" OnClick="ButtonUpd_Click" />
        <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />         
                <asp:BoundField DataField="adornmentName" HeaderText="adornmentName" SortExpression="adornmentName" />
                <asp:BoundField DataField="price" HeaderText="price" SortExpression="price" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
        <br />
        <br />
        <asp:Button ID="ButtonBack" runat="server" Text="Вернуться" OnClick="ButtonBack_Click" />
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetList" TypeName="JewelShopService.ImplementationsDB.AdornmentServiceDB"></asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
