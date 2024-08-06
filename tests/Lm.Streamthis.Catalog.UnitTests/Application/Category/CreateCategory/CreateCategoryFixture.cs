using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.UnitTests.Application.Category.Common;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.CreateCategory;

public class CreateCategoryFixture : CategoryBaseFixture
{
    public CreateCategoryRequest GetRequest() =>
        new(
            GetCategoryName(),
            GetCategoryDescription(),
            GetBoolean());

    public CreateCategoryRequest GetInvalidRequestNullName()
    {
        var requestWithNullName = GetRequest();
        requestWithNullName.Name = null!;
        return requestWithNullName;
    }

    public CreateCategoryRequest GetInvalidRequestShortName()
    {
        var requestWithShortName = GetRequest();
        requestWithShortName.Name = requestWithShortName.Name[..2];
        return requestWithShortName;
    }

    public CreateCategoryRequest GetInvalidRequestLongName()
    {
        var requestWithLongName = GetRequest();
        while (requestWithLongName.Name.Length <= 255)
            requestWithLongName.Name = $"{requestWithLongName.Name} {Faker.Commerce.ProductName()}";
        return requestWithLongName;
    }

    public CreateCategoryRequest GetInvalidRequestNullDescription()
    {
        var requestWithNullDescription = GetRequest();
        requestWithNullDescription.Description = null!;
        return requestWithNullDescription;
    }

    public CreateCategoryRequest GetInvalidRequestLongDescription
        ()
    {
        var requestWithLongDescription = GetRequest();
        while (requestWithLongDescription.Description.Length <= 10_000)
            requestWithLongDescription.Description =
                $"{requestWithLongDescription.Description} {Faker.Commerce.ProductName()}";
        return requestWithLongDescription;
    }
}

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryFixtureCollection
    : ICollectionFixture<CreateCategoryFixture>
{ }
