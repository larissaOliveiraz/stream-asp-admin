using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.Domain.Repositories;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategory(ICategoryRepository repository, IUnitOfWork unitOfWork) : IUpdateCategory
{
    public async Task<CategoryResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await repository.Get(request.Id, cancellationToken);
        
        category.Update(request.Name, request.Description);

        if (request.IsActive != null && category.IsActive != request.IsActive)
            if ((bool)request.IsActive!) 
                category.Activate();
            else 
                category.Deactivate();

        await repository.Update(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt);
    }
}