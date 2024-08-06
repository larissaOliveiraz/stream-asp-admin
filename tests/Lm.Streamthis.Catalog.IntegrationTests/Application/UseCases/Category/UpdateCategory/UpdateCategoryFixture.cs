using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryFixture : CategoryBaseFixture
{
    public UpdateCategoryRequest GetRequest(Guid? id = null) =>
        new UpdateCategoryRequest(
            id ?? Guid.NewGuid(),
            GetCategoryName(),
            GetCategoryDescription(),
            GetBoolean());

    public UpdateCategoryRequest GetInvalidRequestShortName()
    {
        var requestWithShortName = GetRequest();
        requestWithShortName.Name = requestWithShortName.Name[..2];
        return requestWithShortName;
    }

    public UpdateCategoryRequest GetInvalidRequestLongName()
    {
        var requestWithLongName = GetRequest();
        while (requestWithLongName.Name.Length <= 255)
            requestWithLongName.Name = $"{requestWithLongName.Name} {Faker.Commerce.ProductName()}";
        return requestWithLongName;
    }

    public UpdateCategoryRequest GetInvalidRequestLongDescription
        ()
    {
        var requestWithLongDescription = GetRequest();
        while (requestWithLongDescription.Description?.Length <= 10_000)
            requestWithLongDescription.Description =
                $"{requestWithLongDescription.Description} {Faker.Commerce.ProductName()}";
        return requestWithLongDescription;
    }
}

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryFixture> { }
