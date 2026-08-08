namespace Test.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    public override string ToString() => $"[Order] Id: {Id}, CustomerId: {CustomerId}, Total: {TotalAmount:C}, Status: {Status}, Date: {OrderDate:yyyy-MM-dd}";
}