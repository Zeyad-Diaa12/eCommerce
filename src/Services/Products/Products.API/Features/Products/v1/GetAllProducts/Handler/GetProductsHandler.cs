using LinqKit;

namespace Products.API.Features.Products.v1.GetAllProducts.Handler;

public class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Product> productsQuery = session.Query<Product>();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerms = query.Search
                                   .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(term => term.ToLower())
                                   .ToArray();

            foreach (var term in searchTerms)
            {
                productsQuery = productsQuery.Where(product =>
                    product.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    product.Description.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    product.Category.Any(c => c.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                );
            }
        }
        var pagedProducts = await productsQuery
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductsResult
        {
            Records = pagedProducts,
            PageNumber = pagedProducts.PageNumber,
            PageSize = pagedProducts.PageSize,
            NumberOfRecordsFound = pagedProducts.TotalItemCount,
            NumberOfRecordsInPage = pagedProducts.Count,
            NumberOfPages = pagedProducts.PageCount,
            HasNextPage = pagedProducts.HasNextPage,
            HasPreviousPage = pagedProducts.HasPreviousPage
        };
    }
}
