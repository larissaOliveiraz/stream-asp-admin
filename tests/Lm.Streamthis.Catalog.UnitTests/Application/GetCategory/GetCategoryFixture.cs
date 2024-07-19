using Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;
using Lm.Streamthis.Catalog.UnitTests.Application.Common;

namespace Lm.Streamthis.Catalog.UnitTests.Application.GetCategory;

public class GetCategoryFixture : CategoryBaseFixture
{
    public GetCategoryRequest GetRequest(Guid? id = null)
    {
        return new GetCategoryRequest(id ?? Guid.NewGuid());
    }
}

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryFixture>
{
}