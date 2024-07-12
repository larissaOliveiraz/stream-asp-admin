using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Domain.Repositories;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategory(ICategoryRepository repository, IUnitOfWork unitOfWork) : IDeleteCategory
{
    public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await repository.Get(request.Id, cancellationToken);

        await repository.Delete(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }
}