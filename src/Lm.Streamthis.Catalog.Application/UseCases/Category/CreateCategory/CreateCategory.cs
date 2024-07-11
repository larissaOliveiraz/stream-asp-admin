using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Domain.Repositories;
using MediatR;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryResponse> Handle(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = new DomainEntities.Category(
            request.Name, 
            request.Description, 
            request.IsActive);

        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new CreateCategoryResponse(
            category.Id, 
            category.Name, 
            category.Description, 
            category.IsActive, 
            category.CreatedAt);
    }
}