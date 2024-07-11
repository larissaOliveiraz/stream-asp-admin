using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryRequest(string name, string? description = null, bool isActive = true)
    : IRequest<CategoryResponse>
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description ?? "";
    public bool IsActive { get; set; } = isActive;
}