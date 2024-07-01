using Lm.Streamthis.Catalog.Domain.Exceptions;

namespace Lm.Streamthis.Catalog.Domain.Entities;

public class Category
{
    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        
        Validate();
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }    
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be null or empty.");
    }
}