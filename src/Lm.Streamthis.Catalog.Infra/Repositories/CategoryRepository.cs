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

    public async Task<SearchResponse<Category>> Search(SearchRequest request, CancellationToken cancellationToken)
    {
        var skipAmount = (request.Page - 1) * request.PerPage;

        var query = Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x => x.Name.Contains(request.Search));

        query = OrderQuery(query, request.OrderBy, request.Order);

        var items = await query
            .Skip(skipAmount)
            .Take(request.PerPage)
            .ToListAsync(cancellationToken);
        var total = await query.CountAsync(cancellationToken);

        return new SearchResponse<Category>(
            request.Page,
            request.PerPage,
            items, total);
    }

    private IQueryable<Category> OrderQuery(
        IQueryable<Category> query, string orderBy, SearchOrder order) =>
        (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdAt", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name)
        };
}
