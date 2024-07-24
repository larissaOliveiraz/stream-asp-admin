using Lm.Streamthis.Catalog.Application.Exceptions;
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
        await Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? 
        throw new NotFoundException($"Category with id '{id}' was not found.");

    public Task Update(Category category, CancellationToken _) =>
        Task.FromResult(Categories.Update(category));

    public Task Delete(Category category, CancellationToken _) => 
        Task.FromResult(Categories.Remove(category));

    public async Task<SearchResponse<Category>> Search(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var items = await Categories.ToListAsync(cancellationToken);
        var total = await Categories.CountAsync(cancellationToken);

        return new SearchResponse<Category>(
            searchRequest.Page,
            searchRequest.PerPage,
            items,
            total);
    }
}
