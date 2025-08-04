using System.Collections.Generic;

public class RestaurantOrderService
{
    private readonly RestaurantOrderRepository _repo;

    public RestaurantOrderService()
    {
        _repo = new RestaurantOrderRepository();
    }

    public List<RestaurantOrder> GetOrders(int restaurantId, string paymentFilter, string statusFilter, int pageIndex, int pageSize, string sortDirection)
    {
        int startRow = pageIndex * pageSize + 1;
        int endRow = (pageIndex + 1) * pageSize;

        return _repo.GetOrders(restaurantId, paymentFilter, statusFilter, startRow, endRow, sortDirection);
    }

    public int GetTotalOrderPages(int restaurantId, string paymentFilter, string statusFilter, int pageSize)
    {
        int totalRecords = _repo.GetOrderCount(restaurantId, paymentFilter, statusFilter);
        return (int)System.Math.Ceiling((double)totalRecords / pageSize);
    }

    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        _repo.UpdateOrderStatus(orderId, newStatus);
    }
}
