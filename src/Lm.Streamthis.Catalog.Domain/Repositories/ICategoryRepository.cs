using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;

namespace Lm.Streamthis.Catalog.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>, ISearchableRepository<Category>
{
}