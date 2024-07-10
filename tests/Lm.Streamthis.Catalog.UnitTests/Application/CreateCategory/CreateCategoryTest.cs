using Moq;
using FluentAssertions;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using UseCases = Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryTest(CreateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Create_Category))]
    [Trait("Application", "Create Category")]
    public async void Should_Create_Category()
    {
        var repositoryMock = fixture.GetMockRepository;
        var unitOfWorkMock = fixture.GetMockUnitOfWork;
        
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        var input = fixture.GetValidInput();
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(
                It.IsAny<Category>(), 
                It.IsAny<CancellationToken>()
            ), 
            Times.Once);
        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()), 
            Times.Once);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name))]
    [Trait("Application ", "Create Category")]
    public async void Should_Create_Category_Only_With_Name()
    {
        var repositoryMock = fixture.GetMockRepository;
        var unitOfWorkMock = fixture.GetMockUnitOfWork;

        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new CreateCategoryInput(fixture.GetValidInput().Name);
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Insert(
                It.IsAny<Category>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()));
        output.Name.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().NotBeNull();
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name_And_Description))]
    [Trait("Application ", "Create Category")]
    public async void Should_Create_Category_Only_With_Name_And_Description()
    {
        var repositoryMock = fixture.GetMockRepository;
        var unitOfWorkMock = fixture.GetMockUnitOfWork;

        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var generalInput = fixture.GetValidInput();
        var input = new CreateCategoryInput(generalInput.Name, generalInput.Description);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Name.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().NotBeNull();
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Initialization_IsUnsuccessful))]
    [Trait("Application", "Create Category")]
    [MemberData(nameof(GetInvalidInputs))]
    public async void Should_Throw_Error_When_Initialization_IsUnsuccessful(
        CreateCategoryInput invalidInput, 
        string exceptionMessage)
    {
        var useCase = new UseCases.CreateCategory(
            fixture.GetMockRepository.Object, 
            fixture.GetMockUnitOfWork.Object);
        
        var action = async () => 
            await useCase.Handle(invalidInput, CancellationToken.None);

        await action.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryFixture();
        var invalidInputList = new List<object[]>();

        var inputWithShortName = fixture.GetValidInput();
        inputWithShortName.Name = inputWithShortName.Name[..2];
        
        var inputWithLongName = fixture.GetValidInput();
        while (inputWithLongName.Name.Length <= 255)
            inputWithLongName.Name = $"{inputWithLongName.Name} {fixture.Faker.Commerce.ProductName()}";

        var inputWithNullName = fixture.GetValidInput();
        inputWithNullName.Name = null!;
        
        var inputWithLongDescription = fixture.GetValidInput();
        while (inputWithLongDescription.Description.Length <= 10_000)
            inputWithLongDescription.Description = 
                $"{inputWithLongDescription.Description} {fixture.Faker.Commerce.ProductName()}";

        var inputWithNullDescription = fixture.GetValidInput();
        inputWithNullDescription.Description = null!;

        invalidInputList.Add([inputWithShortName, "Name should not have less than 3 characters."]);
        invalidInputList.Add([inputWithLongName, "Name should not have more than 255 characters."]);
        invalidInputList.Add([inputWithNullName, "Name should not be null or empty."]);
        invalidInputList.Add([inputWithLongDescription, "Description should not have more than 10000 characters."]);
        invalidInputList.Add([inputWithNullDescription, "Description should not be null."]);
        
        return invalidInputList;
    }
}