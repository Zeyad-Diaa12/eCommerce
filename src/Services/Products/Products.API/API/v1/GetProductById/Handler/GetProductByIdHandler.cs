namespace Products.API.API.v1.GetProductById.Handler;

public class GetProductByIdQueryHandler
    (IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetproductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetproductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);

        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null) {
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product);
    }
}
