using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

public class ListCategoriesFixture : CategoryBaseFixture
{
    public List<DomainEntities.Category> GetCategoriesListWithNames(List<string> names) =>
        names.Select(name =>
        {
            var category = GetCategory();
            category.Update(name);
            return category;
        }).ToList();
}

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoryFixtureCollection : ICollectionFixture<ListCategoriesFixture> { }
