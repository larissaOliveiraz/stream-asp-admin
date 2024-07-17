using Lm.Streamthis.Catalog.Application.Common;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategoriesRequest(int page, int perPage, string search, string sort, SearchOrder order) 
    : PaginatedListRequest(page, perPage, search, sort, order), IRequest<ListCategoriesResponse>
{
}