namespace Test.Models;

public class Supplier
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public override string ToString() => $"[Supplier] Id: {Id}, Company: {CompanyName}, Contact: {ContactName}, Country: {Country}, Active: {IsActive}";
}