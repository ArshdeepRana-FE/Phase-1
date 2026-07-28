using System.Collections.Generic;
/// <summary>
/// RestuarantOrder represents a Restaurants order
/// </summary>
public class RestaurantOrder
{
    public string OrderID { get; set; }
    public string OrderTime { get; set; }
    public string PaymentMode { get; set; }
    public string Status { get; set; }
    public string CustomerID { get; set; }

    /// <summary>
    /// Order Items is a list of menu items of a particular restaurant
    /// </summary>
    public List<OrderItem> OrderItems { get; set; }

    public decimal TotalPrice { get; set; }
}
