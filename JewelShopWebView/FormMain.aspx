<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormMain.aspx.cs" Inherits="JewelShopWebView.FormMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #form1 {
            height: 666px;
            width: 1067px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Menu ID="Menu" runat="server" BackColor="White" ForeColor="Black" Height="150px">
            <Items>
                <asp:MenuItem Text="Справочники" Value="Справочники">
                    <asp:MenuItem Text="Клиенты" Value="Клиенты" NavigateUrl="~/FormBuyers.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Компоненты" Value="Компоненты" NavigateUrl="~/FormElements.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Изделия" Value="Изделия" NavigateUrl="~/FormAdornments.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Склады" Value="Склады" NavigateUrl="~/FormHangars.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Сотрудники" Value="Сотрудники" NavigateUrl="~/FormCustomers.aspx"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Пополнить склад" Value="Пополнить склад" NavigateUrl="~/FormPutOnHangar.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Button ID="ButtonCreateIndent" runat="server" Text="Создать заказ" OnClick="ButtonCreateIndent_Click" />
        <asp:Button ID="ButtonTakeIndentInWork" runat="server" Text="Отдать на выполнение" OnClick="ButtonTakeIndentInWork_Click" />
        <asp:Button ID="ButtonIndentReady" runat="server" Text="Заказ готов" OnClick="ButtonIndentReady_Click" />
        <asp:Button ID="ButtonIndentPayed" runat="server" Text="Заказ оплачен" OnClick="ButtonIndentPayed_Click" />
        <asp:Button ID="ButtonUpd" runat="server" Text="Обновить список" OnClick="ButtonUpd_Click" />
        <asp:GridView ID="dataGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" />
                <asp:CommandField ShowSelectButton="true" SelectText=">>" /> 
                <asp:BoundField DataField="buyerId" HeaderText="buyerId" SortExpression="buyerId" />
                <asp:BoundField DataField="buyerName" HeaderText="buyerName" SortExpression="buyerName" />
                <asp:BoundField DataField="adornmentId" HeaderText="adornmentId" SortExpression="adornmentId" />
                <asp:BoundField DataField="adornmentName" HeaderText="adornmentName" SortExpression="adornmentName" />
                <asp:BoundField DataField="customerId" HeaderText="customerId" SortExpression="customerId" />
                <asp:BoundField DataField="customerName" HeaderText="customerName" SortExpression="customerName" />
                <asp:BoundField DataField="count" HeaderText="count" SortExpression="count" />
                <asp:BoundField DataField="sum" HeaderText="sum" SortExpression="sum" />
                <asp:BoundField DataField="status" HeaderText="status" SortExpression="status" />
                <asp:BoundField DataField="DateCreate" HeaderText="DateCreate" SortExpression="DateCreate" />
                <asp:BoundField DataField="DateCustom" HeaderText="DateCustom" SortExpression="DateCustom" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="JewelShopService.BindingModels.ProdOrderBindingModel" DeleteMethod="PayOrder" InsertMethod="CreateOrder" SelectMethod="GetList" TypeName="JewelShopService.ImplementationsDB.MainServiceDB" UpdateMethod="TakeOrderInWork">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
