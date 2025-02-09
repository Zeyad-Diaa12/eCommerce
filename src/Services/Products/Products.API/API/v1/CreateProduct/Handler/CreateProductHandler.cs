using BuildingBlocks.CQRS.Command;
using MediatR;
using Products.API.Models;

namespace Products.API.API.v1.CreateProduct.Handler;

public class CreateProductCommandHandler
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price,
            ImageFile = command.ImageFile,
            Description = command.Description,
            Category = command.Category
        };

        return new CreateProductResult(product.Id);
    }
}
