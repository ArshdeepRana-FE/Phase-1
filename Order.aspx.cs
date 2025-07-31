using Newtonsoft.Json;  
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Order : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int orderId = Int32.Parse(Request.QueryString["id"]);

                string orderQuery = "SELECT oi.item_id, oi.quantity, mi.name, mi.price, mi.type " +
                                    "FROM order_item oi " +
                                    "JOIN menu_items mi ON oi.item_id = mi.id " +
                                    "WHERE oi.order_id = @OrderId";

                SqlCommand command = new SqlCommand(orderQuery, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);

                SqlDataReader reader = command.ExecuteReader();

                decimal totalPrice = 0;
                List<OrderItem> orderItems = new List<OrderItem>();

                while (reader.Read())
                {
                    string itemName = reader["name"].ToString();
                    decimal itemPrice = Convert.ToDecimal(reader["price"]);
                    int quantity = Convert.ToInt32(reader["quantity"]);
                    decimal itemTotalPrice = itemPrice * quantity;
                    string itemType = reader["type"].ToString();

                    orderItems.Add(new OrderItem
                    {
                        Name = itemName,
                        Price = itemPrice,
                        Quantity = quantity,
                        TotalPrice = itemTotalPrice,
                        Type = itemType
                    });

                    totalPrice += itemTotalPrice;
                }

                reader.Close();
                connection.Close();

                orderItemsTable.DataSource = orderItems;
                orderItemsTable.DataBind();

                totalPriceLabel.Text = "Total Price: $" + totalPrice.ToString("F2");
            }
        }
    }
}

