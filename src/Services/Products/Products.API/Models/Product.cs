namespace Products.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; } = default;
    public string ImageFile { get; set; } = default;
    public string Description { get; set; } = default;
    public List<string> Category { get; set; } = new();
    public int Stock { get; set; } = default;
}
