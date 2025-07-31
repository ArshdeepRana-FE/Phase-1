<%@ Page Title="Restaurant Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RestaurantPage.aspx.cs" Inherits="RestaurantPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        ORDERS OF THIS RESTAURANT
        <select id="orderFilter" onchange="filterOrders()">
            <option disabled selected>Mode of payment</option>
            <option value="all">All</option>
            <option value="card">Card</option>
            <option value="upi">UPI</option>
        </select>

        <select id="statusFilter" onchange="filterOrders()">
            <option disabled selected>Order status</option>
            <option value="all">All</option>
            <option value="accepted">Accepted</option>
            <option value="delivered">Delivered</option>
            <option value="rejected">Rejected</option>
        </select>

        <asp:GridView ID="OrderGridView" runat="server" AutoGenerateColumns="False" BorderWidth="1px" CellPadding="5"
                      AllowPaging="True" PageSize="5" OnPageIndexChanging="OrderGridView_PageIndexChanging" OnRowDataBound="OrderGridView_RowDataBound">
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID" SortExpression="OrderID" />
                <asp:BoundField DataField="OrderTime" HeaderText="Order Time" SortExpression="OrderTime" />
                <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode" SortExpression="PaymentMode" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:DropDownList ID="StatusDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StatusDropDown_SelectedIndexChanged">
                            <asp:ListItem Text="Accepted" Value="accepted" />
                            <asp:ListItem Text="Delivered" Value="delivered" />
                            <asp:ListItem Text="Rejected" Value="rejected" />
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Save">
                    <ItemTemplate>
                        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" CommandArgument='<%# Eval("OrderID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" SortExpression="CustomerID" />
                <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="lblOrderID" runat="server" Visible="false" Text='<%# Eval("OrderID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Items">
                    <ItemTemplate>
                        <a href="Order.aspx?id=<%# Eval("OrderID") %>">Click to view Items of this order.</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:PlaceHolder ID="OrderPlaceholder" runat="server"></asp:PlaceHolder>
    </div>

    <script>
        function filterOrders() {
            var selectedPaymentFilter = document.getElementById("orderFilter").value;
            var selectedStatusFilter = document.getElementById("statusFilter").value;
            window.location.href = "RestaurantPage.aspx?id=<%= Request.QueryString["id"] %>&filterPayment=" + selectedPaymentFilter + "&filterStatus=" + selectedStatusFilter;
        }
    </script>
</asp:Content>
