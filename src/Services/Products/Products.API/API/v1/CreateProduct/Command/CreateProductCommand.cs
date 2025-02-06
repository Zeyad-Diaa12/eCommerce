namespace Products.API.API.v1.CreateProduct.Request;

public record CreateProductCommand(
    decimal Price, 
    string Name, 
    string ImageFile, 
    string Description, 
    List<string> Category
);
