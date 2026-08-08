namespace Test.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int QuantityInStock { get; set; }
    public string WarehouseLocation { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public override string ToString() => $"[InventoryItem] Id: {Id}, ProductId: {ProductId}, Qty: {QuantityInStock}, Location: {WarehouseLocation}, Updated: {LastUpdated:yyyy-MM-dd}";
}