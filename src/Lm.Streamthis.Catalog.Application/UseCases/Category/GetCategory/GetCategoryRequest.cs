using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryRequest(Guid id) : IRequest<CategoryResponse>
{
    public Guid Id { get; set; } = id;
}