using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

public class ListCategoriesFixture : CategoryBaseFixture
{
}

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoryFixtureCollection : ICollectionFixture<ListCategoriesFixture> { }
