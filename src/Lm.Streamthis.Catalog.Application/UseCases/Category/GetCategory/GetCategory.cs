using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.Domain.Repositories;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory(ICategoryRepository repository) : IGetCategory
{
    public async Task<CategoryResponse> Handle(
        GetCategoryRequest request, 
        CancellationToken cancellationToken)
    {
        var category = await repository.Get(request.Id, cancellationToken);

        return new CategoryResponse(
            category.Id, 
            category.Name, 
            category.Description, 
            category.IsActive,
            category.CreatedAt);
    }
}