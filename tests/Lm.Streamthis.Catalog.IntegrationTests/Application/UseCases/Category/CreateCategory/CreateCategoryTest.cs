using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Infra.Repositories;
using Lm.Streamthis.Catalog.Infra;
using FluentAssertions;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryTest(CreateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Create_Category))]
    [Trait("Integration - Application", "Create Category")]
    public async void Should_Create_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var category = fixture.GetCategory();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new CreateCategoryRequest(category.Name, category.Description, category.IsActive);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        response.CreatedAt.Should().NotBeSameDateAs(default);

        var createdCategory = await fixture.CreateDbContext(true).Categories().FindAsync(response.Id);
        createdCategory.Should().NotBeNull();
        createdCategory.Name.Should().Be(request.Name);
        createdCategory.Description.Should().Be(request.Description);
        createdCategory.IsActive.Should().Be(request.IsActive);
        createdCategory.CreatedAt.Should().Be(response.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name))]
    [Trait("Integration - Application", "Create Category")]
    public async void Should_Create_Category_Only_With_Name()
    {
        var dbContext = fixture.CreateDbContext();
        var category = fixture.GetCategory();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new CreateCategoryRequest(category.Name);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be("");
        response.IsActive.Should().BeTrue();
        response.CreatedAt.Should().NotBeSameDateAs(default);

        var createdCategory = await fixture.CreateDbContext(true).Categories().FindAsync(response.Id);
        createdCategory.Should().NotBeNull();
        createdCategory.Name.Should().Be(request.Name);
        createdCategory.Description.Should().Be("");
        createdCategory.IsActive.Should().BeTrue();
        createdCategory.CreatedAt.Should().Be(response.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name_And_Description))]
    [Trait("Integration - Application", "Create Category")]
    public async void Should_Create_Category_Only_With_Name_And_Description()
    {
        var dbContext = fixture.CreateDbContext();
        var category = fixture.GetCategory();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new CreateCategoryRequest(category.Name, category.Description);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().BeTrue();
        response.CreatedAt.Should().NotBeSameDateAs(default);

        var createdCategory = await fixture.CreateDbContext(true).Categories().FindAsync(response.Id);
        createdCategory.Should().NotBeNull();
        createdCategory.Name.Should().Be(request.Name);
        createdCategory.Description.Should().Be(request.Description);
        createdCategory.IsActive.Should().BeTrue();
        createdCategory.CreatedAt.Should().Be(response.CreatedAt);
    }

    [Theory(DisplayName = nameof(Should_Throw_Exception_When_Request_IsInvalid))]
    [Trait("Integration - Application", "Create Category")]
    [MemberData(
       nameof(CreateCategoryDataGenerator.GetInvalidRequests),
       parameters: 5,
       MemberType = typeof(CreateCategoryDataGenerator))]
    public async void Should_Throw_Exception_When_Request_IsInvalid(
        CreateCategoryRequest invalidRequest,
        string exceptionMessage)
    {
        var dbContext = fixture.CreateDbContext();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var action = async () =>
            await useCase.Handle(invalidRequest, CancellationToken.None);

        await action.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);

        var dbCategoriesList = fixture.CreateDbContext(true).Categories().AsNoTracking().ToList();
        dbCategoriesList.Should().HaveCount(0);
    }
}
