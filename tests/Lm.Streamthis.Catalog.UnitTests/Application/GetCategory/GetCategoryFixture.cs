using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.UnitTests.Common;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.GetCategory;

public class GetCategoryFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetMockRepository() => new();

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
    
    public Category GetValidCategory() =>
        new(GetValidCategoryName(),GetValidCategoryDescription());
}

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryFixture>
{
}