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
        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null.");
        switch (Name.Length)
        {
            case < 3:
                throw new EntityValidationException(
                    $"{nameof(Name)} should be at least three characters long."
                );
            case > 255:
                throw new EntityValidationException(
                    $"{nameof(Name)} should be equal or less than 255 characters long.");
        }

        if (Description.Length > 10000)
            throw new EntityValidationException(
                $"{nameof(Description)} should be equal or less than 10.000 characters long."
            );
    }
}