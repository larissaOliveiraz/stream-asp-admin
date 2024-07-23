using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.UnitTests.Common;
using Moq;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.Common;

public class CategoryBaseFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetMockRepository() => new();
    
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new();
    
    protected string GetValidCategoryName()
    {
        var categoryName = "";
        
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        
        return categoryName;
    }

    protected string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        
        return categoryDescription;
    }
    
    protected static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    
    public DomainEntities.Category GetValidCategory() =>
        new(GetValidCategoryName(),GetValidCategoryDescription(), GetRandomBoolean());
}