using BuildingBlocks.CQRS.Command;
using MediatR;

namespace Products.API.API.v1.CreateProduct.Handler;

public record CreateProductCommand(
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
) : ICommand<CreateProductResult>;
