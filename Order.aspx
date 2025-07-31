<%@ Page Title="Order Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Order" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Order Details</h2>

    <!-- Table to display the order items -->
    <asp:GridView ID="orderItemsTable" runat="server" AutoGenerateColumns="False" BorderWidth="1px" CellPadding="5">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Item Name" SortExpression="Name" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:F2}" />
            <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" DataFormatString="{0:F2}" />
            <asp:BoundField DataField="Type" HeaderText="Type" />
        </Columns>
    </asp:GridView>

    <!-- Label to display total price -->
    <p><strong><asp:Label ID="totalPriceLabel" runat="server" Text="Total Price: $0.00"></asp:Label></strong></p>
</asp:Content>
