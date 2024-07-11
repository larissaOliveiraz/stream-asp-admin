namespace Lm.Streamthis.Catalog.Domain.SeedWork;

public interface IRepository<TAggregate> : IGenericRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> Get(Guid id, CancellationToken cancellationToken);

}