using MediatR;
using Products.API.API.v1.CreateProduct.Request;
using Products.API.API.v1.CreateProduct.Result;

namespace Products.API.Endpoints.v1.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        throw new NotImplementedException();
    }
}
