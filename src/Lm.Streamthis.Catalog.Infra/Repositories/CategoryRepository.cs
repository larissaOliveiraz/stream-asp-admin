using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.Infra.Repositories;

public class CategoryRepository(StreamAspDbContext context) : ICategoryRepository
{
    private DbSet<Category> Categories => context.Set<Category>();

    public async Task Insert(Category category, CancellationToken cancellationToken) =>
        await Categories.AddAsync(category, cancellationToken);

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken) =>
        await Categories.FindAsync([id], cancellationToken);

    public Task<SearchResponse<Category>> Search(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
