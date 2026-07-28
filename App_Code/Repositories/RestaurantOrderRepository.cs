using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// RestaurantOrderRepository : Repository class responsible for handling restaurant orders.
/// </summary>
public class RestaurantOrderRepository
{
    /// <summary>
    /// Executes SQL queries to fetch orders of this restaurant and order items of those orders
    /// </summary>
    /// <param name="restaurantId"></param>
    /// <param name="paymentFilter"></param>
    /// <param name="statusFilter"></param>
    /// <param name="startRow"></param>
    /// <param name="endRow"></param>
    /// <param name="sortDirection"></param>
    /// <param name="searchText"></param>
    /// <returns></returns>
    public List<RestaurantOrder> GetOrders(
        int restaurantId,
        string paymentFilter,
        string statusFilter,
        int startRow,
        int endRow,
        string sortDirection,
        string searchText
        )
    {
        var orders = new List<RestaurantOrder>();

        // Validate sortDirection
        sortDirection = sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";

        string query = RestaurantOrderQueries.PaginatedOrders(sortDirection);

        if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter.ToLower() != "all")
            query += RestaurantOrderQueries.PaymentFilter;

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter.ToLower() != "all")
            query += RestaurantOrderQueries.StatusFilter;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query += RestaurantOrderQueries.SearchByItemNameOrOrderID;
        }

        query += RestaurantOrderQueries.GetOrderItems(sortDirection);


        var parameters = new List<SqlParameter>
        {
            new SqlParameter("@RestaurantId", restaurantId),
            new SqlParameter("@StartRow", startRow),
            new SqlParameter("@EndRow", endRow)
        };

        if (!string.IsNullOrEmpty(paymentFilter) && paymentFilter.ToLower() != "all")
            parameters.Add(new SqlParameter("@PaymentFilter", paymentFilter));

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter.ToLower() != "all")
            parameters.Add(new SqlParameter("@StatusFilter", statusFilter));

        if (!string.IsNullOrWhiteSpace(searchText))
            parameters.Add(new SqlParameter("@SearchText", searchText));

        using (var reader = DBUtil.ExecuteReader(query, parameters.ToArray()))
        {
            var orderDict = new Dictionary<string, RestaurantOrder>();

            while (reader.Read())
            {
                string orderId = reader["order_id"].ToString();

                if (!orderDict.ContainsKey(orderId))
                {
                    orderDict[orderId] = new RestaurantOrder
                    {
                        OrderID = orderId,
                        OrderTime = reader["order_time"].ToString(),
                        PaymentMode = reader["mode_of_payment"].ToString() == "False" ? "UPI" : "Card", 
                        Status = reader["status"].ToString(),
                        CustomerID = reader["customer_id"].ToString(),
                        OrderItems = new List<OrderItem>()
                    };
                }

                var order = orderDict[orderId];
                order.OrderItems.Add(new OrderItem
                {
                    Id = Convert.ToInt32(reader["item_id"]),
                    Quantity = Convert.ToInt32(reader["quantity"]),
                    Name = reader["name"].ToString(),
                    Price = Convert.ToDecimal(reader["price"]),
                    Type = reader["type"].ToString(),
                    TotalPrice = Convert.ToDecimal(reader["price"]) * Convert.ToInt32(reader["quantity"])
                });
            }

            orders = new List<RestaurantOrder>(orderDict.Values);
        }

        return orders;
    }

    /// <summary>
    /// Executes SQL query to get total number of orders of a particular restaurant for pagination purposes.
    /// </summary>
    /// <param name="restaurantId"></param>
    /// <param name="paymentFilter"></param>
    /// <param name="statusFilter"></param>
    /// <returns></returns>
    public int GetOrderCount(int restaurantId, string paymentFilter, string statusFilter, string searchText)
    {
            List<string> conditions = new List<string> { RestaurantOrderQueries.RetaurantIdMatches };

            if (paymentFilter != "all")
                conditions.Add(RestaurantOrderQueries.PaymentFilter);

            if (statusFilter != "all")
                conditions.Add(RestaurantOrderQueries.StatusFilter);

            if (!string.IsNullOrEmpty(searchText))
            {
                conditions.Add(RestaurantOrderQueries.OrderWithItemName);
            }

            string whereClause = string.Join(" ", conditions);

            string sql = RestaurantOrderQueries.GetTotalOrders(whereClause);

        var parameters = new List<SqlParameter>
            {
                new SqlParameter("@RestaurantId", restaurantId)
            };

              if (paymentFilter != "all")
                parameters.Add(new SqlParameter("@PaymentFilter", paymentFilter));
              if (statusFilter != "all")
                parameters.Add(new SqlParameter("@StatusFilter", statusFilter));
              if (!string.IsNullOrEmpty(searchText))
                parameters.Add(new SqlParameter("@SearchText", searchText));

        return (int)DBUtil.ExecuteScalar(sql, parameters.ToArray());
    }
    

    /// <summary>
    /// Executes a SQL query to update order status
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="newStatus"></param>
    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        string query = RestaurantOrderQueries.UpdateStatus;

        SqlParameter[] parameters =
        {
            new SqlParameter("@NewStatus", newStatus),
            new SqlParameter("@OrderId", orderId)
        };

        DBUtil.ExecuteNonQuery(query, parameters);
    }
}
