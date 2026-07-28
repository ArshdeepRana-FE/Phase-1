using Microsoft.Ajax.Utilities;

/// <summary>
/// Class containing SQL queries related to restaurant orders.
/// This class provides SQL queries for fetching orders, updating order status, 
/// filtering orders, and pagination of orders for a given restaurant.
/// </summary>
public class RestaurantOrderQueries
{
    /// <summary>
    /// Generates a SQL query to fetch paginated orders for a given restaurant.
    /// The query orders the results by order time in either ascending or descending order
    /// based on the specified sort direction.
    /// </summary>
    /// <param name="sortDirection">The direction to sort the orders, either 'ASC' or 'DESC'.</param>
    /// <summary>
    /// Builds the opening SQL statement for retrieving a restaurant's orders with row-based pagination.
    /// </summary>
    /// <param name="sortDirection">The SQL sort direction applied to the order time.</param>
    /// <returns>A SQL query fragment that starts the paginated orders common table expression.</returns>
    public static string PaginatedOrders(string sortDirection)
    {
        return $@"
        WITH PaginatedOrders AS (
            SELECT
                ROW_NUMBER() OVER (ORDER BY o.order_time {sortDirection}) AS RowNum,
                o.id,
                o.restaurant_id,
                o.order_time,
                o.mode_of_payment,
                o.status,
                o.customer_id
            FROM orders o
            WHERE o.restaurant_id = @RestaurantId
        ";
    }

    /// <summary>
    /// SQL condition to filter orders based on the mode of payment.
    /// </summary>
    public const string PaymentFilter = " AND o.mode_of_payment = @PaymentFilter";

    /// <summary>
    /// SQL condition to filter orders based on the order status.
    /// </summary>
    public const string StatusFilter = " AND o.status = @StatusFilter";

    public const string RetaurantIdMatches = " o.restaurant_id = @RestaurantId";
    /// <summary>
    /// SQL condition to filter orders based on a search text for the item name.
    /// This condition checks if any order item or orderId contains the search text in its name.
    /// </summary>
    public const string SearchByItemNameOrOrderID = @"
    AND EXISTS (
        SELECT 1
        FROM order_item oi
        INNER JOIN menu_items mi ON oi.item_id = mi.id
        WHERE oi.order_id = o.id
        AND mi.name LIKE '%' + @SearchText + '%'
        OR o.id LIKE '%' + @SearchText + '%'
    )";

    /// <summary>
    /// SQL query to help find the orders with matching order items
    /// </summary>
    public const string OrderWithItemName = @"
               AND EXISTS (
                    SELECT 1 FROM order_item oi
                    INNER JOIN menu_items mi ON oi.item_id = mi.id
                    WHERE oi.order_id = o.id AND mi.name LIKE '%' + @SearchText + '%'
                )
               ";
    /// <summary>
    /// Generates a SQL query to fetch order items for the paginated orders.
    /// The query retrieves the order details along with the items in those orders.
    /// </summary>
    /// <param name="sortDirection">The direction to sort the results, either 'ASC' or 'DESC'.</param>
    /// <summary>
    /// Completes the paginated order query by selecting each order's items within the requested row range.
    /// </summary>
    /// <param name="sortDirection">The SQL sort direction for ordering orders by order time.</param>
    /// <returns>A SQL query fragment containing the order and item data.</returns>
    public static string GetOrderItems(string sortDirection)
    {
        return $@"
        )
        SELECT
            po.RowNum,
            po.id AS order_id,
            po.restaurant_id,
            po.order_time,
            po.mode_of_payment,
            po.status,
            po.customer_id,
            oi.item_id,
            oi.quantity,
            mi.name,
            mi.price,
            mi.type
        FROM PaginatedOrders po
        INNER JOIN order_item oi ON po.id = oi.order_id
        INNER JOIN menu_items mi ON oi.item_id = mi.id
        WHERE po.RowNum BETWEEN @StartRow AND @EndRow
        ORDER BY po.order_time {sortDirection}, oi.item_id;";
    }

    /// <summary>
    /// SQL query to update the status of an order.
    /// </summary>
    public const string UpdateStatus = "UPDATE orders SET status = @NewStatus WHERE id = @OrderId";

    /// <summary>
    /// SQL query to count the total number of orders for a given restaurant.
    /// <summary>
    /// Builds a query that counts orders matching the specified condition.
    /// </summary>
    /// <param name="whereClause">The SQL condition applied to the orders.</param>
    /// <returns>A SQL query that returns the matching order count.</returns>
    public static string GetTotalOrders(string whereClause)
    {
        return $"SELECT COUNT(*) FROM orders o WHERE {whereClause}";
    }
}
