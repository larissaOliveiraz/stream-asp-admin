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

    public CreateCategoryInput GetValidInput() =>
        new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());

    public CreateCategoryInput GetInvalidInputNullName()
    {
        var inputWithNullName = GetValidInput();
        inputWithNullName.Name = null!;
        return inputWithNullName;
    }

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var inputWithShortName = GetValidInput();
        inputWithShortName.Name = inputWithShortName.Name[..2];
        return inputWithShortName;
    }

    public CreateCategoryInput GetInvalidInputLongName()
    {
        var inputWithLongName = GetValidInput();
        while (inputWithLongName.Name.Length <= 255)
            inputWithLongName.Name = $"{inputWithLongName.Name} {Faker.Commerce.ProductName()}";
        return inputWithLongName;
    }

    public CreateCategoryInput GetInvalidInputNullDescription()
    {
        var inputWithNullDescription = GetValidInput();
        inputWithNullDescription.Description = null!;
        return inputWithNullDescription;
    }

    public CreateCategoryInput GetInvalidInputLongDescription()
    {
        var inputWithLongDescription = GetValidInput();
        while (inputWithLongDescription.Description.Length <= 10_000)
            inputWithLongDescription.Description = 
                $"{inputWithLongDescription.Description} {Faker.Commerce.ProductName()}";
        return inputWithLongDescription;
    }

    public Mock<ICategoryRepository> GetMockRepository => new();
    
    public Mock<IUnitOfWork> GetMockUnitOfWork => new();
}

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryFixtureCollection 
    : ICollectionFixture<CreateCategoryFixture>
{ }