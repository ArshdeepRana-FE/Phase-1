using System.Collections.Generic;

/// <summary>
/// Service class for RestaurantOrder
/// </summary>
public class RestaurantOrderService
{
    private readonly RestaurantOrderRepository _repo;

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
    /// <returns></returns>
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
    /// <returns></returns>
    public int GetTotalOrderPages(int restaurantId, string paymentFilter, string statusFilter, int pageSize)
    {
        int totalRecords = _repo.GetOrderCount(restaurantId, paymentFilter, statusFilter);
        return (int)System.Math.Ceiling((double)totalRecords / pageSize);
    }

    /// <summary>
    /// This method is crucial to update status of orders
    /// </summary>
    /// <param name="orderId">Unique Order id</param>
    /// <param name="newStatus">Updated status</param>
    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        _repo.UpdateOrderStatus(orderId, newStatus);
    }
}
