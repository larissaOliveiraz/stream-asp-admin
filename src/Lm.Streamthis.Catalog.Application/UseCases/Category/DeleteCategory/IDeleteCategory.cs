using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryRequest>
{
}