using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class OrderRepository
{
    public List<OrderItem> GetOrderItemsByOrderId(int orderId)
    {
        var items = new List<OrderItem>();

        string query = @"
            SELECT oi.item_id, oi.quantity, mi.name, mi.price, mi.type
            FROM order_item oi
            JOIN menu_items mi ON oi.item_id = mi.id
            WHERE oi.order_id = @OrderId";

        using (SqlConnection conn = new SqlConnection(DBUtil.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            conn.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    decimal price = Convert.ToDecimal(reader["price"]);
                    int quantity = Convert.ToInt32(reader["quantity"]);

                    items.Add(new OrderItem
                    {
                        Name = reader["name"].ToString(),
                        Price = price,
                        Quantity = quantity,
                        TotalPrice = price * quantity,
                        Type = reader["type"].ToString()
                    });
                }
            }
        }

        return items;
    }
}
