namespace Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

public interface ISearchableRepository<TAggregate> 
    where TAggregate : AggregateRoot
{
    Task<SearchResponse<TAggregate>> Search(SearchRequest searchRequest, CancellationToken cancellationToken);
}