using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using Moq;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest(UpdateCategoryFixture fixture)
{
    [Theory(DisplayName = nameof(Should_Update_Category))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetValidCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category(DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        repositoryMock
            .Setup(x =>
                x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);
        repositoryMock.Verify(repository =>
                repository.Get(category.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repository =>
                repository.Update(category, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(uow =>
                uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory(DisplayName = nameof(Should_Update_Only_With_Name))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetValidCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Only_With_Name(DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        repositoryMock
            .Setup(x =>
                x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var requestOnlyWithName = new UpdateCategoryRequest(
            category.Id,
            request.Name);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);
        var response = await useCase.Handle(requestOnlyWithName, CancellationToken.None);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(category.Description);
        response.IsActive.Should().Be(category.IsActive);
        repositoryMock.Verify(repository =>
                repository.Get(category.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repository =>
                repository.Update(category, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(uow =>
                uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory(DisplayName = nameof(Should_Update_Category_Without_IsActive))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetValidCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async Task Should_Update_Category_Without_IsActive(
        DomainEntities.Category category,
        UpdateCategoryRequest request)
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        repositoryMock
            .Setup(x =>
                x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var requestWithoutIsActive = new UpdateCategoryRequest(
            request.Id,
            request.Name,
            request.Description);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);
        var response = await useCase.Handle(requestWithoutIsActive, CancellationToken.None);

        response.Should().NotBeNull();
        response.Name.Should().Be(requestWithoutIsActive.Name);
        response.Description.Should().Be(requestWithoutIsActive.Description);
        response.IsActive.Should().Be(category.IsActive);
        repositoryMock.Verify(repository =>
                repository.Get(request.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repository =>
                repository.Update(category, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(uow =>
                uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Application", "Update Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        var request = fixture.GetRequest();
        repositoryMock
            .Setup(x =>
                x.Get(request.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category with id '{request.Id}' was not found."));

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);

        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository =>
                repository.Get(request.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory(DisplayName = nameof(Should_Throw_Exception_When_Request_IsInvalid))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetInvalidRequests),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Throw_Exception_When_Request_IsInvalid(
        UpdateCategoryRequest invalidRequest,
        string exceptionMessage)
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();
        var validCategory = fixture.GetValidCategory();
        invalidRequest.Id = validCategory.Id;

        repositoryMock
            .Setup(x =>
                x.Get(invalidRequest.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validCategory);

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);
        var action = async () =>
            await useCase.Handle(invalidRequest, CancellationToken.None);

        await action.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
        repositoryMock.Verify(repository =>
                repository.Get(invalidRequest.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}