using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryRequest(Guid id) : IRequest<GetCategoryResponse>
{
    public Guid Id { get; set; } = id;
}