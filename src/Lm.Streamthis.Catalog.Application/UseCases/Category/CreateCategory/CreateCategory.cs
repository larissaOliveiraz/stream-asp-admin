using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Domain.Repositories;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICreateCategory
{
    public async Task<CreateCategoryResponse> Handle(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = new DomainEntities.Category(
            request.Name, 
            request.Description, 
            request.IsActive);

        await categoryRepository.Insert(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return new CreateCategoryResponse(
            category.Id, 
            category.Name, 
            category.Description, 
            category.IsActive, 
            category.CreatedAt);
    }
}