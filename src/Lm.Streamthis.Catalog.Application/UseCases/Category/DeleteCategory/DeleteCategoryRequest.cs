using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryRequest(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}