using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Lm.Streamthis.Catalog.UnitTests.Common;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.ListCategories;

public class ListCategoriesFixture : BaseFixture
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

    public List<Category> GetValidCategoryList(int? length = 10)
    {
        var categoryList = new List<Category>();
        
        for (var i = 0; i < length; i++)
            categoryList.Add(GetValidCategory());

        return categoryList;
    }

    public ListCategoriesRequest GetValidRequest()
    {
        var randomNumber = new Random();
        return new ListCategoriesRequest(
            randomNumber.Next(1, 10),
            randomNumber.Next(15, 100),
            Faker.Commerce.ProductName(),
            Faker.Commerce.ProductName(),
            randomNumber.Next(1, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc);
    }

    public Mock<ICategoryRepository> GetMockRepository() => new();
}

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoriesFixtureCollection : ICollectionFixture<ListCategoriesFixture>
{
}