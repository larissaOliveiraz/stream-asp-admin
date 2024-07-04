using DomainEntity = Lm.Streamthis.Catalog.Domain.Entities;
namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory() => 
        new("category name", "category description");
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture>
{
}