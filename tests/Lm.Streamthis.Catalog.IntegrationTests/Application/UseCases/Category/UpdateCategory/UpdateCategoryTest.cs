using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.Infra.Repositories;
using Lm.Streamthis.Catalog.Infra;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Domain.Exceptions;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest(UpdateCategoryFixture fixture)
{
    [Theory(DisplayName = nameof(Should_Update_Category))]
    [Trait("Integration - Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
        parameters: 3,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category(
        DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetCategoriesList(10));
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var unitOfWork = new UnitOfWork(dbContext);

        var repository = new CategoryRepository(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(category.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        var updatedCategory = await fixture.CreateDbContext(true).Categories().FindAsync(response.Id);
        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(response.Name);
        updatedCategory.Description.Should().Be(response.Description);
        updatedCategory.IsActive.Should().Be((bool)response.IsActive);
        updatedCategory.CreatedAt.Should().Be(response.CreatedAt);
    }

    [Theory(DisplayName = nameof(Should_Update_Category_Without_IsActive))]
    [Trait("Integration - Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category_Without_IsActive(
        DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var dbContext = fixture.CreateDbContext();

        await dbContext.AddRangeAsync(fixture.GetCategoriesList(10));
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var requestWithoutIsActive = new UpdateCategoryRequest(
            request.Id,
            request.Name,
            request.Description);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var response = await useCase.Handle(requestWithoutIsActive, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(category.IsActive);

        var updatedCategory = await fixture.CreateDbContext(true).Categories().FindAsync(request.Id);
        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(request.Name);
        updatedCategory.Description.Should().Be(request.Description);
        updatedCategory.IsActive.Should().Be(category.IsActive);
        updatedCategory.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Theory(DisplayName = nameof(Should_Update_Category_Only_With_Name))]
    [Trait("Integration - Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category_Only_With_Name(
        DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var dbContext = fixture.CreateDbContext();

        await dbContext.AddRangeAsync(fixture.GetCategoriesList(10));
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var requestOnlyWithName = new UpdateCategoryRequest(
            request.Id,
            request.Name);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var response = await useCase.Handle(requestOnlyWithName, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(category.Description);
        response.IsActive.Should().Be(category.IsActive);

        var updatedCategory = await fixture.CreateDbContext(true).Categories().FindAsync(request.Id);
        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(request.Name);
        updatedCategory.Description.Should().Be(category.Description);
        updatedCategory.IsActive.Should().Be(category.IsActive);
        updatedCategory.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Integration - Application", "Update Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var dbContext = fixture.CreateDbContext();

        await dbContext.AddRangeAsync(fixture.GetCategoriesList(10));
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new UpdateCategoryRequest(
            Guid.NewGuid(),
            fixture.GetCategoryName());

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category with id '{request.Id}' was not found.");
    }

    [Theory(DisplayName = nameof(Should_Throw_Exception_When_Request_IsInvalid))]
    [Trait("Integration - Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetInvalidRequests),
        parameters: 5,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Throw_Exception_When_Request_IsInvalid(
        UpdateCategoryRequest invalidRequest, string exceptionMessage)
    {
        var dbContext = fixture.CreateDbContext();

        var categoriesList = fixture.GetCategoriesList(10);
        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync();
        invalidRequest.Id = categoriesList[0].Id;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var action = async () =>
            await useCase.Handle(invalidRequest, CancellationToken.None);

        await action.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
