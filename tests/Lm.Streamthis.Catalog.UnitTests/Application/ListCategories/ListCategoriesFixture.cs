using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Lm.Streamthis.Catalog.UnitTests.Application.Common;

namespace Lm.Streamthis.Catalog.UnitTests.Application.ListCategories;

public class ListCategoriesFixture : CategoryBaseFixture
{
    public List<Category> GetValidCategoryList(int? length = 15)
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
}

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoriesFixtureCollection : ICollectionFixture<ListCategoriesFixture>
{
}