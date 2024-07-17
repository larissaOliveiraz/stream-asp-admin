using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

namespace Lm.Streamthis.Catalog.Application.Common;

public abstract class PaginatedListRequest(int page, int perPage, string search, string sort, SearchOrder order) 
{
    public int Page { get; set; } = page;
    public int PerPage { get; set; } = perPage;
    public string Search { get; set; } = search;
    public string Sort { get; set; } = sort;
    public SearchOrder Order { get; set; } = order;
}