using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryFixture : CategoryBaseFixture
{
}

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryFixture> { }
