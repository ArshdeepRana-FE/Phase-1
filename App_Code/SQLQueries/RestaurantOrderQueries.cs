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
    /// <returns>A SQL query string for fetching paginated orders.</returns>
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

    /// <summary>
    /// SQL condition to filter orders based on a search text for the item name.
    /// This condition checks if any order item contains the search text in its name.
    /// </summary>
    public const string SearchByItemName = @"
    AND EXISTS (
        SELECT 1
        FROM order_item oi
        INNER JOIN menu_items mi ON oi.item_id = mi.id
        WHERE oi.order_id = o.id
        AND mi.name LIKE '%' + @SearchText + '%'
    )";

    /// <summary>
    /// Generates a SQL query to fetch order items for the paginated orders.
    /// The query retrieves the order details along with the items in those orders.
    /// </summary>
    /// <param name="sortDirection">The direction to sort the results, either 'ASC' or 'DESC'.</param>
    /// <returns>A SQL query string for fetching order items for paginated orders.</returns>
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
    /// </summary>
    public const string TotalOrders = "SELECT COUNT(*) FROM orders o WHERE restaurant_id = @RestaurantId";
}
