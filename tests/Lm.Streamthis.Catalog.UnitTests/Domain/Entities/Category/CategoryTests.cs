using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

public class CategoryTests
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntities.Category(validData.Name, validData.Description);

        Assert.NotNull(category);
        Assert.Equal(category.Name, validData.Name);
        Assert.Equal(category.Description, validData.Description);
    }
}