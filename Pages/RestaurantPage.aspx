<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RestaurantPage.aspx.cs" Inherits="RestaurantPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Restaurant Orders</title>
    <style>
        .filters {
            margin: 20px;
        }

        .order-card {
            border: 1px solid #ccc;
            padding: 15px;
            margin: 20px;
            border-radius: 6px;
            box-shadow: 1px 1px 5px #ccc;
        }

        .order-header {
            margin-bottom: 10px;
        }

        .order-header label {
            display: inline-block;
            margin-right: 15px;
        }

        .order-items {
            margin-top: 10px;
            width: 100%;
            border-collapse: collapse;
        }

        .order-items th, .order-items td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
        }

        .status-dropdown {
            width: 100px;
        }

         .no-results {
            padding: 20px;
            color: #666;
            font-size: 18px;
            text-align: center;
         }

        .pagination a {
            margin: 0 5px;
            text-decoration: none;
            padding: 5px 10px;
            border: 1px solid #ccc;
        }

        .pagination a:hover {
            background-color: #eee;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="filters">
            <asp:Label ID="lblFilters" runat="server" Text="Filter by:" Font-Bold="true" />
            <br /><br />
            <asp:Label ID="lblPayment" runat="server" Text="Payment:" />
            <asp:DropDownList ID="ddlPaymentFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
                <asp:ListItem Text="All" Value="all" />
                <asp:ListItem Text="UPI" Value="0" />
                <asp:ListItem Text="Card" Value="1" />
            </asp:DropDownList>

            <asp:Label ID="lblStatus" runat="server" Text="Status:" />
            <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
                <asp:ListItem Text="All" Value="all" />
                <asp:ListItem Text="Rejected" Value="0" />
                <asp:ListItem Text="Accepted" Value="1" />
                <asp:ListItem Text="Delivered" Value="2" />
            </asp:DropDownList>

            <asp:Label ID="lblSort" runat="server" Text="Sort by Time:" />
            <asp:DropDownList ID="ddlSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
                <asp:ListItem Text="Newest First" Value="desc" />
                <asp:ListItem Text="Oldest First" Value="asc" />
            </asp:DropDownList>
             <br /><br />
            <asp:Label ID="lblSearch" runat="server" Text="Search by Order item name:" />
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bar" />
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="SearchItem"/>
            

           </div>

        <asp:PlaceHolder ID="OrdersPanel" runat="server" />
        <asp:PlaceHolder ID="PaginationPanel" runat="server" />
    </form>
</body>
</html>
