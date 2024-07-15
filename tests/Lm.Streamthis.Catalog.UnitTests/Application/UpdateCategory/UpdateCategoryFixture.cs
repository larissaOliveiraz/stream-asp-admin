using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.UnitTests.Common;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";
        
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        
        return categoryDescription;
    }
    
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    
    public Category GetValidCategory() =>
        new(GetValidCategoryName(),
            GetValidCategoryDescription(), 
            GetRandomBoolean());

    public UpdateCategoryRequest GetValidRequest(Guid? id = null) =>
        new(id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());
    
    public Mock<ICategoryRepository> GetMockRepository() => new();
    
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new();
}

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryFixture>
{
}