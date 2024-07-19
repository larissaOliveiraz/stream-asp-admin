using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.UnitTests.Application.Common;

namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryFixture : CategoryBaseFixture
{
    public CreateCategoryRequest GetValidRequest() =>
        new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());

    public CreateCategoryRequest GetInvalidRequestNullName()
    {
        var requestWithNullName = GetValidRequest();
        requestWithNullName.Name = null!;
        return requestWithNullName;
    }

    public CreateCategoryRequest GetInvalidRequestShortName()
    {
        var requestWithShortName = GetValidRequest();
        requestWithShortName.Name = requestWithShortName.Name[..2];
        return requestWithShortName;
    }

    public CreateCategoryRequest GetInvalidRequestLongName()
    {
        var requestWithLongName = GetValidRequest();
        while (requestWithLongName.Name.Length <= 255)
            requestWithLongName.Name = $"{requestWithLongName.Name} {Faker.Commerce.ProductName()}";
        return requestWithLongName;
    }

    public CreateCategoryRequest GetInvalidRequestNullDescription()
    {
        var requestWithNullDescription = GetValidRequest();
        requestWithNullDescription.Description = null!;
        return requestWithNullDescription;
    }

    public CreateCategoryRequest GetInvalidRequestLongDescription
        ()
    {
        var requestWithLongDescription = GetValidRequest();
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