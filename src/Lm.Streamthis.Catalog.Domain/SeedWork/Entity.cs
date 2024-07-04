namespace Lm.Streamthis.Catalog.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
}