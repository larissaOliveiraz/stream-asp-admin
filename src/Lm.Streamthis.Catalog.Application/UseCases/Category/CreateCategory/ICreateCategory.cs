using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
} 