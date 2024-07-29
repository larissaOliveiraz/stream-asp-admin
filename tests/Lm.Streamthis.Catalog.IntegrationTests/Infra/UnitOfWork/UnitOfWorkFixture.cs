using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.IntegrationTests.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Infra.UnitOfWork;

public class UnitOfWorkFixture : BaseFixture
{
    private string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    private string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    private static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public Category GetValidCategory() =>
        new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());


    public List<Category> GetValidCategoryList(int length) =>
        Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();
}

[CollectionDefinition(nameof(UnitOfWorkFixture))]
public class UnitOfWorkFixtureCollection : ICollectionFixture<UnitOfWorkFixture> { }
