using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategories(ICategoryRepository repository) : IListCategories
{
    public async Task<ListCategoriesResponse> Handle(ListCategoriesRequest request, CancellationToken cancellationToken)
    {
        var searchRequest = new SearchRequest(
            request.Page,
            request.PerPage,
            request.Search,
            request.Sort,
            request.Order);
        
        var searchResponse = await repository.Search(searchRequest, cancellationToken);

        return new ListCategoriesResponse(
            searchResponse.CurrentPage,
            searchResponse.PerPage,
            searchResponse.Total,
            searchResponse.Items
                .Select(item => 
                    new CategoryResponse(
                        item.Id,
                        item.Name,
                        item.Description,
                        item.IsActive,
                        item.CreatedAt)
                ).ToList());
    }
}