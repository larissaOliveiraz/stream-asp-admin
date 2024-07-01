namespace Lm.Streamthis.Catalog.Domain.Entities;

public class Category
{
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
}