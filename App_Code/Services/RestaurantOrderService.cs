using System.Collections.Generic;

/// <summary>
/// Service class for RestaurantOrder
/// </summary>
public class RestaurantOrderService
{
    private readonly RestaurantOrderRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestaurantOrderService"/> class.
    /// </summary>
    public RestaurantOrderService()
    {
        _repo = new RestaurantOrderRepository();
    }

    /// <summary>
    /// This method plays a crucial role for pagination
    /// </summary>
    /// <param name="restaurantId">Unique id of restaurant</param>
    /// <param name="paymentFilter">Payment filter i.e., UPI or Card</param>
    /// <param name="statusFilter">Status filter i.e, Rejected, Accepted or Delivered</param>
    /// <param name="pageIndex">Page index (page user is currently on)</param>
    /// <param name="pageSize">Number of orders per page</param>
    /// <param name="sortDirection">Sorting way i.e., asc or desc</param>
    /// <param name="searchText">Search text : Text entered by user in the search bar</param>
    /// <summary>
    /// Retrieves a paginated list of orders matching the specified filters.
    /// </summary>
    /// <param name="paymentFilter">The payment filter to apply.</param>
    /// <param name="statusFilter">The order status filter to apply.</param>
    /// <param name="pageIndex">The zero-based page index.</param>
    /// <param name="pageSize">The maximum number of orders per page.</param>
    /// <param name="sortDirection">The direction in which to sort the orders.</param>
    /// <param name="searchText">The text used to search for matching orders.</param>
    /// <returns>The orders matching the filters for the specified page.</returns>
    public List<RestaurantOrder> GetOrders(int restaurantId, string paymentFilter, string statusFilter, int pageIndex, int pageSize, string sortDirection, string searchText)
    {
        int startRow = pageIndex * pageSize + 1;
        int endRow = (pageIndex + 1) * pageSize;

        return _repo.GetOrders(restaurantId, paymentFilter, statusFilter, startRow, endRow, sortDirection, searchText);
    }

    /// <summary>
    /// This method plays a crucial role for pagination
    /// </summary>
    /// <param name="restaurantId">Unique id of restaurant</param>
    /// <param name="paymentFilter">Payment filter i.e., UPI or Card</param>
    /// <param name="statusFilter">Status filter i.e, Rejected, Accepted or Delivered</param>
    /// <param name="pageSize">Number of orders per page</param>
    /// <summary>
    /// Calculates the number of pages available for the filtered restaurant orders.
    /// </summary>
    /// <param name="restaurantId">The identifier of the restaurant.</param>
    /// <param name="paymentFilter">The payment status filter.</param>
    /// <param name="statusFilter">The order status filter.</param>
    /// <param name="pageSize">The maximum number of orders per page.</param>
    /// <param name="searchText">The text used to filter matching orders.</param>
    /// <returns>The total number of pages required for the matching orders.</returns>
    public int GetTotalOrderPages(int restaurantId, string paymentFilter, string statusFilter, int pageSize, string searchText)
    {
        int totalRecords = _repo.GetOrderCount(restaurantId, paymentFilter, statusFilter, searchText);
        return (int)System.Math.Ceiling((double)totalRecords / pageSize);
    }

    /// <summary>
    /// This method is crucial to update status of orders
    /// </summary>
    /// <param name="orderId">Unique Order id</param>
    /// <summary>
    /// Updates the status of a specific restaurant order.
    /// </summary>
    /// <param name="orderId">The identifier of the order to update.</param>
    /// <param name="newStatus">The new status for the order.</param>
    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        _repo.UpdateOrderStatus(orderId, newStatus);
    }
}
