namespace Lm.Streamthis.Catalog.Domain.SeedWork;

public interface IRepository<in TAggregate> : IGenericRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);

}