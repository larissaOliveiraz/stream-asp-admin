using Lm.Streamthis.Catalog.UnitTests.Common;
using DomainEntity = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

public class CategoryFixture : BaseFixture
{
    public string GetCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    private string GetCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public DomainEntity.Category GetCategory() =>
        new(GetCategoryName(), GetCategoryDescription());
}

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryFixture>
{
}
