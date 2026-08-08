namespace Test.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public override string ToString() => $"[Product] Id: {Id}, Name: {Name}, Price: {Price}, CreatedAt: {CreatedAt:yyyy-MM-dd}";
}