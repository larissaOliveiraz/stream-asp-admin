using Lm.Streamthis.Catalog.Domain.Repositories;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory(ICategoryRepository repository) : IGetCategory
{
    public async Task<GetCategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await repository.Get(request.Id, cancellationToken);

        return new GetCategoryResponse(
            category.Id, 
            category.Name, 
            category.Description, 
            category.IsActive,
            category.CreatedAt);
    }
}