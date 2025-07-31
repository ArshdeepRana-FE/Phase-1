using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RestaurantPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || Session["UserRole"] == null)
        {
            Response.Redirect("Account/Login.aspx");
        }

        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
        string paymentFilter = Request.QueryString["filterPayment"];
        string statusFilter = Request.QueryString["filterStatus"];
        int pageIndex = 0;
        int pageSize = 5;

        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                pageIndex = int.Parse(Request.QueryString["page"]);
            }

            LoadOrders(connectionString, paymentFilter, statusFilter, pageIndex, pageSize);
        }
    }

    private void LoadOrders(string connectionString, string paymentFilter, string statusFilter, int pageIndex, int pageSize)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string orderQuery = @"
                SELECT * 
                FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY order_time DESC) AS RowNum, * 
                    FROM orders 
                    WHERE restaurant_id = @RestaurantId";

            if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            {
                orderQuery += " AND mode_of_payment = @PaymentFilter";
            }

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                orderQuery += " AND status = @StatusFilter";
            }

            orderQuery += @") AS OrdersWithRowNum
                           WHERE RowNum BETWEEN @StartRow AND @EndRow";

            SqlCommand command = new SqlCommand(orderQuery, connection);
            command.Parameters.AddWithValue("@RestaurantId", Int32.Parse(Request.QueryString["id"]));

            if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            {
                command.Parameters.AddWithValue("@PaymentFilter", paymentFilter);
            }

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                command.Parameters.AddWithValue("@StatusFilter", statusFilter);
            }

            int startRow = pageIndex * pageSize + 1;
            int endRow = (pageIndex + 1) * pageSize;

            command.Parameters.AddWithValue("@StartRow", startRow);
            command.Parameters.AddWithValue("@EndRow", endRow);

            SqlDataReader reader = command.ExecuteReader();

            List<RestuarantOrder> orders = new List<RestuarantOrder>();
            while (reader.Read())
            {
                orders.Add(new RestuarantOrder
                {
                    OrderID = reader["id"].ToString(),
                    OrderTime = reader["order_time"].ToString(),
                    PaymentMode = reader["mode_of_payment"].ToString(),
                    Status = reader["status"].ToString(),
                    CustomerID = reader["customer_id"].ToString()
                });
            }

            reader.Close();
            connection.Close();

            OrderGridView.DataSource = orders;
            OrderGridView.DataBind();

            AddPaginationLinks(pageIndex, pageSize, paymentFilter, statusFilter);
        }
    }

    protected void OrderGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList statusDropDown = (DropDownList)e.Row.FindControl("StatusDropDown");

            string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
            if (statusDropDown != null)
            {
                statusDropDown.SelectedValue = status;
            }
        }
    }

    // Update the order status when the dropdown value is changed
    protected void StatusDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList statusDropDown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)statusDropDown.NamingContainer;
        string orderId = ((Label)row.FindControl("lblOrderID")).Text;
        string newStatus = statusDropDown.SelectedValue;

        UpdateOrderStatus(orderId, newStatus);
    }

    // Update the status in the database
    private void UpdateOrderStatus(string orderId, string newStatus)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE orders SET status = @NewStatus WHERE id = @OrderId";
            SqlCommand command = new SqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@NewStatus", newStatus);
            command.Parameters.AddWithValue("@OrderId", orderId);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    // Handle Save button click
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string orderId = btn.CommandArgument;
        DropDownList statusDropDown = (DropDownList)btn.NamingContainer.FindControl("StatusDropDown");
        string newStatus = statusDropDown.SelectedValue;

        UpdateOrderStatus(orderId, newStatus);
        Response.Redirect(Request.Url.ToString()); // Refresh the page
    }

    private void AddPaginationLinks(int pageIndex, int pageSize, string paymentFilter, string statusFilter)
    {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString))
        {
            connection.Open();
            string countQuery = "SELECT COUNT(*) FROM orders WHERE restaurant_id = @RestaurantId";

            if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            {
                countQuery += " AND mode_of_payment = @PaymentFilter";
            }

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                countQuery += " AND status = @StatusFilter";
            }

            SqlCommand countCommand = new SqlCommand(countQuery, connection);
            countCommand.Parameters.AddWithValue("@RestaurantId", Int32.Parse(Request.QueryString["id"]));

            if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            {
                countCommand.Parameters.AddWithValue("@PaymentFilter", paymentFilter);
            }

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                countCommand.Parameters.AddWithValue("@StatusFilter", statusFilter);
            }

            int totalRecords = (int)countCommand.ExecuteScalar();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            connection.Close();

            string paginationHtml = "<div class='pagination'>";

            if (pageIndex > 0)
            {
                paginationHtml += $"<a href='RestaurantPage.aspx?id={Request.QueryString["id"]}&filterPayment={paymentFilter}&filterStatus={statusFilter}&page={pageIndex - 1}'>Prev</a>";
            }

            for (int i = 0; i < totalPages; i++)
            {
                paginationHtml += $"<a href='RestaurantPage.aspx?id={Request.QueryString["id"]}&filterPayment={paymentFilter}&filterStatus={statusFilter}&page={i}'>{i + 1}</a>";
            }

            if (pageIndex < totalPages - 1)
            {
                paginationHtml += $"<a href='RestaurantPage.aspx?id={Request.QueryString["id"]}&filterPayment={paymentFilter}&filterStatus={statusFilter}&page={pageIndex + 1}'>Next</a>";
            }

            paginationHtml += "</div>";
            OrderPlaceholder.Controls.Add(new LiteralControl(paginationHtml));
        }
    }

    protected void OrderGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string paymentFilter = Request.QueryString["filterPayment"];
        string statusFilter = Request.QueryString["filterStatus"];
        Response.Redirect($"RestaurantPage.aspx?id={Request.QueryString["id"]}&filterPayment={paymentFilter}&filterStatus={statusFilter}&page={e.NewPageIndex}");
    }
}
