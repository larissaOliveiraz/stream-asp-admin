namespace Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryInput(string name, string? description = null, bool isActive = true)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description ?? "";
    public bool IsActive { get; set; } = isActive;
}