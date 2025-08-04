using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RestaurantPage : Page
{
    private readonly RestaurantOrderService _orderService = new RestaurantOrderService();
    private readonly OrderService _orderDetailsService = new OrderService();

    private int _restaurantId;
    private int _pageSize = 2;
    private int _pageIndex;
    private string _paymentFilter;
    private string _statusFilter;
    private string _sortOrder;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || Session["UserRole"] == null || Convert.ToInt32(Session["UserRole"]) != 0)
        {
            Response.Redirect("Account/Login.aspx");
            return;
        }

        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("Default.aspx");
            return;
        }

        _restaurantId = int.Parse(Request.QueryString["id"]);
        _paymentFilter = Request.QueryString["filterPayment"] ?? "all";
        _statusFilter = Request.QueryString["filterStatus"] ?? "all";
        _sortOrder = Request.QueryString["sort"] ?? "desc";
        _pageIndex = Request.QueryString["page"] != null ? int.Parse(Request.QueryString["page"]) : 0;

        if (!IsPostBack)
        {
            ddlPaymentFilter.SelectedValue = _paymentFilter;
            ddlStatusFilter.SelectedValue = _statusFilter;
            ddlSortOrder.SelectedValue = _sortOrder;
        }

        LoadOrders();
    }

    private void LoadOrders()
    {
        var orders = _orderService.GetOrders(_restaurantId, _paymentFilter, _statusFilter, _pageIndex, _pageSize, _sortOrder);
        OrdersPanel.Controls.Clear();

        foreach (var order in orders)
        {
            var orderItems = _orderDetailsService.GetItems(Convert.ToInt32(order.OrderID));
            var totalPrice = _orderDetailsService.GetTotalPrice(Convert.ToInt32(order.OrderID));

            Panel card = new Panel { CssClass = "order-card" };

            // Order Header
            card.Controls.Add(new LiteralControl($@"
                <div class='order-header'>
                    <label><strong>Order ID:</strong> {order.OrderID}</label>
                    <label><strong>Time:</strong> {order.OrderTime}</label>
                    <label><strong>Payment Mode:</strong> {order.PaymentMode}</label>
                    <label><strong>Status:</strong></label>
            "));

            // Status dropdown
            DropDownList statusDropdown = new DropDownList { CssClass = "status-dropdown", AutoPostBack = false };
            statusDropdown.Items.Add(new ListItem("Accepted", "1"));
            statusDropdown.Items.Add(new ListItem("Rejected", "0"));
            statusDropdown.Items.Add(new ListItem("Delivered", "2"));
            statusDropdown.SelectedValue = order.Status.ToString();
            statusDropdown.ID = "Status_" + order.OrderID;

            // Save button
            Button saveBtn = new Button
            {
                Text = "Save",
                CommandArgument = order.OrderID.ToString(),
                ID = "Save_" + order.OrderID
            };
            saveBtn.Click += SaveStatus_Click;

            card.Controls.Add(statusDropdown);
            card.Controls.Add(saveBtn);
            card.Controls.Add(new LiteralControl("</div>"));

            // Order Items Table
            Table itemsTable = new Table { CssClass = "order-items" };
            TableHeaderRow header = new TableHeaderRow();
            foreach (string col in new[] { "Item Name", "Quantity", "Unit Price", "Total Price", "Type" })
            {
                header.Cells.Add(new TableHeaderCell { Text = col });
            }
            itemsTable.Rows.Add(header);

            foreach (var item in orderItems)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell { Text = item.Name });
                row.Cells.Add(new TableCell { Text = item.Quantity.ToString() });
                row.Cells.Add(new TableCell { Text = $"₹{item.Price:F2}" });
                row.Cells.Add(new TableCell { Text = $"₹{item.TotalPrice:F2}" });
                row.Cells.Add(new TableCell { Text = item.Type });
                itemsTable.Rows.Add(row);
            }

            card.Controls.Add(itemsTable);

            // Total Price
            card.Controls.Add(new LiteralControl($"<br /><strong>Total Price: ₹{totalPrice:F2}</strong>"));

            OrdersPanel.Controls.Add(card);
        }

        AddPaginationLinks();
    }

    protected void SaveStatus_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string orderId = btn.CommandArgument;
        DropDownList ddl = (DropDownList)FindControl("Status_" + orderId);

        if (ddl != null)
        {
            _orderService.UpdateOrderStatus(orderId, ddl.SelectedValue);
            Response.Redirect(Request.Url.ToString()); // Refresh
        }
    }

    protected void FilterChanged(object sender, EventArgs e)
    {
        string newPayment = ddlPaymentFilter.SelectedValue;
        string newStatus = ddlStatusFilter.SelectedValue;
        string newSort = ddlSortOrder.SelectedValue;

        Response.Redirect($"RestaurantPage.aspx?id={_restaurantId}&filterPayment={newPayment}&filterStatus={newStatus}&sort={newSort}&page=0");
    }

    private void AddPaginationLinks()
    {
        int totalPages = _orderService.GetTotalOrderPages(_restaurantId, _paymentFilter, _statusFilter, _pageSize);

        string paginationHtml = "<div class='pagination'>";
        if (_pageIndex > 0)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&page={_pageIndex - 1}'>Prev</a>";
        }

        for (int i = 0; i < totalPages; i++)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&page={i}'>{i + 1}</a>";
        }

        if (_pageIndex < totalPages - 1)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&page={_pageIndex + 1}'>Next</a>";
        }

        paginationHtml += "</div>";

        PaginationPanel.Controls.Clear();
        PaginationPanel.Controls.Add(new LiteralControl(paginationHtml));
    }
}
