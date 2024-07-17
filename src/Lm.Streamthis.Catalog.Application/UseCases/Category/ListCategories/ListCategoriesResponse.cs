using Lm.Streamthis.Catalog.Application.Common;
using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategoriesResponse(int page, int perPage, int total, List<CategoryResponse> items)
    : PaginatedListResponse<CategoryResponse>(page, perPage, total, items) 
{
}