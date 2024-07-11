using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory : IRequestHandler<CreateCategoryRequest, CategoryResponse>
{
} 