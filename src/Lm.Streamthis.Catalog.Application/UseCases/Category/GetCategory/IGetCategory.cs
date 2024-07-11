using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public interface IGetCategory : IRequestHandler<GetCategoryRequest, GetCategoryResponse>
{
}