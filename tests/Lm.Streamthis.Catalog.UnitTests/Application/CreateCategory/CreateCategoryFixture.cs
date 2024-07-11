using Lm.Streamthis.Catalog.Application.Interfaces;
using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Lm.Streamthis.Catalog.UnitTests.Common;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryFixture : BaseFixture
{
    private string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    private string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    private static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

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

    public Mock<ICategoryRepository> GetMockRepository() => new();
    
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new();
}

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryFixtureCollection 
    : ICollectionFixture<CreateCategoryFixture>
{ }