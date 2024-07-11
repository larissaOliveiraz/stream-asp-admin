namespace Lm.Streamthis.Catalog.Application.UseCases.Category.Common;

public class CategoryResponse(Guid id, string name, string description, bool isActive, DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool IsActive { get; set; } = isActive;
    public DateTime CreatedAt { get; set;  } = createdAt;
}