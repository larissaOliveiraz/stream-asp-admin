namespace Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

public class SearchResponse<TAggregate>(int currentPage, int perPage, List<TAggregate> items, int total) 
    where TAggregate : AggregateRoot
{
    public int CurrentPage { get; set; } = currentPage;
    public int PerPage { get; set; } = perPage;
    public List<TAggregate> Items { get; set; } = items;
    public int Total { get; set; } = total;
}