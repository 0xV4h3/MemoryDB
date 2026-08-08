namespace Test.Models;

public static class TestData
{
    public static List<Customer> GetCustomers() =>
    [
        new Customer { Id = 1, FullName = "Alice Smith", Email = "alice@example.com", Phone = "555-0101" },
        new Customer { Id = 2, FullName = "Bob Jones", Email = "bob@example.com", Phone = "555-0102" },
        new Customer { Id = 3, FullName = "Charlie Brown", Email = "charlie@example.com", Phone = "555-0103" }
    ];

    public static List<Order> GetOrders() =>
    [
        new Order { Id = 101, CustomerId = 1, TotalAmount = 59.99m, Status = "Shipped" },
        new Order { Id = 102, CustomerId = 2, TotalAmount = 120.50m, Status = "Processing" },
        new Order { Id = 103, CustomerId = 1, TotalAmount = 15.00m, Status = "Pending" }
    ];

    public static List<Supplier> GetSuppliers() =>
    [
        new Supplier { Id = 1, CompanyName = "Acme Corp", ContactName = "John Doe", Country = "USA" },
        new Supplier { Id = 2, CompanyName = "Global Tech", ContactName = "Anna Schmidt", Country = "Germany" },
        new Supplier { Id = 3, CompanyName = "Pacific Trade", ContactName = "Kenji Sato", Country = "Japan" }
    ];

    public static List<InventoryItem> GetInventoryItems() =>
    [
        new InventoryItem { Id = 1, ProductId = 501, QuantityInStock = 120, WarehouseLocation = "Aisle A1" },
        new InventoryItem { Id = 2, ProductId = 502, QuantityInStock = 45, WarehouseLocation = "Aisle B3" },
        new InventoryItem { Id = 3, ProductId = 503, QuantityInStock = 0, WarehouseLocation = "Aisle C2" }
    ];
    
    public static List<Product> GetProducts() =>
    [
        new Product { Id = 501, Name = "Wireless Mouse", Price = 29.99m, CreatedAt = DateTime.UtcNow.AddDays(-10) },
        new Product { Id = 502, Name = "Mechanical Keyboard", Price = 89.95m, CreatedAt = DateTime.UtcNow.AddDays(-5) },
        new Product { Id = 503, Name = "4K Monitor", Price = 349.99m, CreatedAt = DateTime.UtcNow.AddDays(-2) },
        new Product { Id = 504, Name = "USB-C Hub", Price = 19.99m, CreatedAt = DateTime.UtcNow.AddDays(-30) },
        new Product { Id = 505, Name = "Noise Cancelling Headphones", Price = 199.99m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
    ];
}