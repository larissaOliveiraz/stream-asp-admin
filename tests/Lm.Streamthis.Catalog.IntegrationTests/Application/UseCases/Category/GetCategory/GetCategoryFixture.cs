using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.IntegrationTests.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

public class GetCategoryFixture : CategoryBaseFixture
{

}

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryFixture> { }
