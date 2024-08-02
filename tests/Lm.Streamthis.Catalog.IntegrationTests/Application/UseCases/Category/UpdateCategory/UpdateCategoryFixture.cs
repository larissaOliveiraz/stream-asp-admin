using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryFixture : CategoryBaseFixture
{
    public UpdateCategoryRequest GetRequest(Guid? id = null) =>
        new UpdateCategoryRequest(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());
}

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryFixture> { }
