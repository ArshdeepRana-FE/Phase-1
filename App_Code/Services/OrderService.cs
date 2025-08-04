using System.Collections.Generic;
using System.Linq;

public class OrderService
{
    private readonly OrderRepository _repo = new OrderRepository();

    public List<OrderItem> GetItems(int orderId)
    {
        return _repo.GetOrderItemsByOrderId(orderId);
    }

    public decimal GetTotalPrice(int orderId)
    {
        var items = _repo.GetOrderItemsByOrderId(orderId);
        return items.Sum(i => i.TotalPrice);
    }
}
