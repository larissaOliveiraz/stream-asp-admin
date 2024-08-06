using Lm.Streamthis.Catalog.IntegrationTests.Common;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

public class CategoryBaseFixture : BaseFixture
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

    public string GetCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public static bool GetBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntities.Category GetCategory() =>
        new(GetCategoryName(), GetCategoryDescription(), GetBoolean());


    public List<DomainEntities.Category> GetCategoriesList(int length) =>
        Enumerable.Range(0, length).Select(_ => GetCategory()).ToList();
}
