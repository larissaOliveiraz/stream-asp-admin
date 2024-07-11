using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryRequest(string name, string? description = null, bool isActive = true)
    : IRequest<CreateCategoryResponse>
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description ?? "";
    public bool IsActive { get; set; } = isActive;
}