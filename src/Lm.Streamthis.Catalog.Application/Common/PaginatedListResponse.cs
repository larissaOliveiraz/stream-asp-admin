namespace Lm.Streamthis.Catalog.Application.Common;

public abstract class PaginatedListResponse<TResponse>(int page, int perPage, int total, List<TResponse> items)
{
    public int Page { get; set; } = page;
    public int PerPage { get; set; } = perPage;
    public int Total { get; set; } = total;
    public List<TResponse> Items { get; set; } = items;
}