using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RestaurantPage : Page
{
    private readonly RestaurantOrderService _orderService = new RestaurantOrderService();

    private int _restaurantId;
    private int _pageSize = 1;
    private int _pageIndex;
    private string _paymentFilter;
    private string _statusFilter;
    private string _sortOrder;
    private string _searchText;

    /// <summary>
    /// Event handler for the Page_Load event. This method is executed when the page is first loaded.
    /// It handles user authentication, query string processing, and initial loading of filters.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check for valid user session and role (admin)
        if (Session["UserId"] == null || Session["UserRole"] == null || Convert.ToInt32(Session["UserRole"]) != 0)
        {
            Response.Redirect("Account/Login.aspx");
            return;
        }

        // Redirect to the default page if no restaurant id is provided in the query string
        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("Default.aspx");
            return;
        }

        // Get the query string parameters
        _restaurantId = int.Parse(Request.QueryString["id"]);
        _paymentFilter = Request.QueryString["filterPayment"] ?? "all";
        _statusFilter = Request.QueryString["filterStatus"] ?? "all";
        _sortOrder = Request.QueryString["sort"] ?? "desc";
        _pageIndex = Request.QueryString["page"] != null ? int.Parse(Request.QueryString["page"]) : 0;
        _searchText = Request.QueryString["search"] ?? "";

        // Set default values for the dropdown filters if the page is being loaded for the first time
        if (!IsPostBack)
        {
            ddlPaymentFilter.SelectedValue = _paymentFilter;
            ddlStatusFilter.SelectedValue = _statusFilter;
            ddlSortOrder.SelectedValue = _sortOrder;
        }

        // Load and display the orders
        LoadOrders();
    }

    /// <summary>
    /// Loads the orders for the specified restaurant based on the current filters and pagination.
    /// This method is responsible for generating the HTML for each order and its associated items.
    /// </summary>
    private void LoadOrders()
    {
        // Fetch the list of orders from the service based on current filters and pagination parameters
        List<RestaurantOrder> orders = _orderService.GetOrders(_restaurantId, _paymentFilter, _statusFilter, _pageIndex, _pageSize, _sortOrder, _searchText);
        OrdersPanel.Controls.Clear(); // Clear previous order data from the panel

        // Loop through each order and display its details
        foreach (RestaurantOrder order in orders)
        {
            List<OrderItem> orderItems = order.OrderItems;

            // Create a panel card for the order
            Panel card = new Panel { CssClass = "order-card" };

            // Add order header (Order ID, Time, Payment Mode, and Status)
            card.Controls.Add(new LiteralControl($@"
                <div class='order-header'>
                    <label><strong>Order ID:</strong> {order.OrderID}</label>
                    <label><strong>Time:</strong> {order.OrderTime}</label>
                    <label><strong>Payment Mode:</strong> {order.PaymentMode}</label>
                    <label><strong>Status:</strong></label>
            "));

            // Add status dropdown for updating order status
            DropDownList statusDropdown = new DropDownList { CssClass = "status-dropdown", AutoPostBack = false };
            statusDropdown.Items.Add(new ListItem("Accepted", "1"));
            statusDropdown.Items.Add(new ListItem("Rejected", "0"));
            statusDropdown.Items.Add(new ListItem("Delivered", "2"));
            statusDropdown.SelectedValue = order.Status.ToString();
            statusDropdown.ID = "Status_" + order.OrderID;

            // Add save button for updating the status
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

            // Add each item in the order to the items table
            foreach (OrderItem item in orderItems)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell { Text = item.Name });
                row.Cells.Add(new TableCell { Text = item.Quantity.ToString() });
                row.Cells.Add(new TableCell { Text = $"₹{item.Price:F2}" });
                row.Cells.Add(new TableCell { Text = $"₹{item.TotalPrice:F2}" });
                row.Cells.Add(new TableCell { Text = item.Type });
                itemsTable.Rows.Add(row);

                order.TotalPrice += item.TotalPrice;
            }

            card.Controls.Add(itemsTable);

            // Add total price of the order
            card.Controls.Add(new LiteralControl($"<br /><strong>Total Price: ₹{order.TotalPrice}</strong>"));

            OrdersPanel.Controls.Add(card); // Add the card to the page
        }

        // Add pagination links for navigating between pages
        AddPaginationLinks();
    }

    /// <summary>
    /// Event handler for saving the updated status of an order when the "Save" button is clicked.
    /// It triggers an update in the database and refreshes the page.
    /// </summary>
    protected void SaveStatus_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string orderId = btn.CommandArgument;
        DropDownList ddl = (DropDownList)FindControl("Status_" + orderId);

        if (ddl != null)
        {
            // Update the order status in the database
            _orderService.UpdateOrderStatus(orderId, ddl.SelectedValue);
            Response.Redirect(Request.Url.ToString()); // Refresh the page
        }
    }

    /// <summary>
    /// Event handler for when any filter (payment, status, or sort order) is changed.
    /// It reloads the page with the new filter values and resets the page index to 0.
    /// </summary>
    protected void FilterChanged(object sender, EventArgs e)
    {
        string newPayment = ddlPaymentFilter.SelectedValue;
        string newStatus = ddlStatusFilter.SelectedValue;
        string newSort = ddlSortOrder.SelectedValue;

        // Redirect with updated filters and reset page to the first page
        Response.Redirect($"RestaurantPage.aspx?id={_restaurantId}&filterPayment={newPayment}&filterStatus={newStatus}&sort={newSort}&search={_searchText}&page=0");
    }

    /// <summary>
    /// Adds pagination links at the bottom of the page to navigate between order pages.
    /// It calculates the total number of pages and creates the corresponding navigation links.
    /// </summary>
    private void AddPaginationLinks()
    {
        int totalPages = _orderService.GetTotalOrderPages(_restaurantId, _paymentFilter, _statusFilter, _pageSize);

        string paginationHtml = "<div class='pagination'>";
        if (_pageIndex > 0)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&search={_searchText}&page={_pageIndex - 1}'>Prev</a>";
        }

        for (int i = 0; i < totalPages; i++)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&search={_searchText}&page={i}'>{i + 1}</a>";
        }

        if (_pageIndex < totalPages - 1)
        {
            paginationHtml += $"<a href='RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&search={_searchText}&page={_pageIndex + 1}'>Next</a>";
        }

        paginationHtml += "</div>";

        PaginationPanel.Controls.Clear();
        PaginationPanel.Controls.Add(new LiteralControl(paginationHtml));
    }

    /// <summary>
    /// Event handler for the search input. It updates the search text and reloads the page with the search filter applied.
    /// </summary>
    protected void SearchItem(object sender, EventArgs e)
    {
        _searchText = txtSearch.Text.Trim();
        // Redirect to the first page with the updated search text
        Response.Redirect($"RestaurantPage.aspx?id={_restaurantId}&filterPayment={_paymentFilter}&filterStatus={_statusFilter}&sort={_sortOrder}&search={_searchText}&page=0");
    }
}
