using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Lm.Streamthis.Catalog.UnitTests.Application.Category.Common;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.ListCategories;

public class ListCategoriesFixture : CategoryBaseFixture
{
    public List<DomainEntities.Category> GetCategoriesList(int? length = 15)
    {
        var categoryList = new List<DomainEntities.Category>();

        for (var i = 0; i < length; i++)
            categoryList.Add(GetCategory());

        return categoryList;
    }

    public ListCategoriesRequest GetRequest()
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
