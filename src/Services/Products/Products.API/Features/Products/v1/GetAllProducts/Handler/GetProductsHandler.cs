namespace Products.API.Features.Products.v1.GetAllProducts.Handler;

public class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        var result = new GetProductsResult()
        {
            Records = products,
            PageNumber = products.PageNumber,
            PageSize = products.PageSize,
            NumberOfRecordsReturned = products.TotalItemCount,
            NumberOfRecordsInPage = products.Count,
            NumberOfPages = products.PageCount,
            HasNextPage = products.HasNextPage,
            HasPreviousPage = products.HasPreviousPage,
        };

        return result;
    }
}
