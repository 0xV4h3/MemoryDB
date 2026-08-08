namespace Test.Models;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    
    public override string ToString() => $"[Customer] Id: {Id}, Name: {FullName}, Email: {Email}, Phone: {Phone}, RegisteredAt: {RegisteredAt:yyyy-MM-dd}";
}