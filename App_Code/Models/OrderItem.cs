/// <summary>
/// Order Item represents an item contained in an order.
/// </summary>
public class OrderItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string Type { get; set; }
}
