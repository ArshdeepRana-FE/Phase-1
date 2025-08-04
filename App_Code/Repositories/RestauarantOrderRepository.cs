using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class RestaurantOrderRepository
{
    public List<RestaurantOrder> GetOrders(int restaurantId, string paymentFilter, string statusFilter, int startRow, int endRow, string sortDirection)
    {
        var orders = new List<RestaurantOrder>();

        string query = $@"
            SELECT * FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY order_time {sortDirection}) AS RowNum, * 
                FROM orders 
                WHERE restaurant_id = @RestaurantId";

        if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            query += " AND mode_of_payment = @PaymentFilter";

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            query += " AND status = @StatusFilter";

        query += ") AS OrdersWithRowNum WHERE RowNum BETWEEN @StartRow AND @EndRow";

        var parameters = new List<SqlParameter>
        {
            new SqlParameter("@RestaurantId", restaurantId),
            new SqlParameter("@StartRow", startRow),
            new SqlParameter("@EndRow", endRow)
        };

        if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
            parameters.Add(new SqlParameter("@PaymentFilter", paymentFilter));

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            parameters.Add(new SqlParameter("@StatusFilter", statusFilter));

        using (var reader = DBUtil.ExecuteReader(query, parameters.ToArray()))
        {
            while (reader.Read())
            {
                orders.Add(new RestaurantOrder
                {
                    OrderID = reader["id"].ToString(),
                    OrderTime = reader["order_time"].ToString(),
                    PaymentMode = reader["mode_of_payment"].ToString() == "False" ? "UPI" : "Card",
                    Status = reader["status"].ToString(),
                    CustomerID = reader["customer_id"].ToString()
                });
            }
        }

        return orders;
    }

    public int GetOrderCount(int restaurantId, string paymentFilter, string statusFilter)
    {
        string query = "SELECT COUNT(*) FROM orders WHERE restaurant_id = @RestaurantId";

        var parameters = new List<SqlParameter>
        {
            new SqlParameter("@RestaurantId", restaurantId)
        };

        if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter != "all")
        {
            query += " AND mode_of_payment = @PaymentFilter";
            parameters.Add(new SqlParameter("@PaymentFilter", paymentFilter));
        }

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
        {
            query += " AND status = @StatusFilter";
            parameters.Add(new SqlParameter("@StatusFilter", statusFilter));
        }

        return Convert.ToInt32(DBUtil.ExecuteScalar(query, parameters.ToArray()));
    }

    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        string query = "UPDATE orders SET status = @NewStatus WHERE id = @OrderId";

        SqlParameter[] parameters = {
            new SqlParameter("@NewStatus", newStatus),
            new SqlParameter("@OrderId", orderId)
        };

        DBUtil.ExecuteNonQuery(query, parameters);
    }
}
