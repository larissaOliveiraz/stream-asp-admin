using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using Moq;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.CreateCategory;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryTest(CreateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Create_Category))]
    [Trait("Unit - Application", "Create Category")]
    public async void Should_Create_Category()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWorkMock = fixture.GetMockUnitOfWork();

        var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var request = fixture.GetRequest();
        var response = await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(
                It.IsAny<DomainEntities.Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        response.Id.Should().NotBeEmpty();
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name))]
    [Trait("Unit - Application", "Create Category")]
    public async void Should_Create_Category_Only_With_Name()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWorkMock = fixture.GetMockUnitOfWork();

        var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var request = new CreateCategoryRequest(fixture.GetRequest().Name);
        var response = await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(
                It.IsAny<DomainEntities.Category>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()));
        response.Name.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().NotBeNull();
        response.Description.Should().Be("");
        response.IsActive.Should().BeTrue();
        response.Id.Should().NotBeEmpty();
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(Should_Create_Category_Only_With_Name_And_Description))]
    [Trait("Unit - Application", "Create Category")]
    public async void Should_Create_Category_Only_With_Name_And_Description()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWorkMock = fixture.GetMockUnitOfWork();

        var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var normalRequest = fixture.GetRequest();
        var request = new CreateCategoryRequest(normalRequest.Name, normalRequest.Description);

        var response = await useCase.Handle(request, CancellationToken.None);

        response.Name.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
        response.Description.Should().NotBeNull();
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().BeTrue();
        response.Id.Should().NotBeEmpty();
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Request_IsInvalid))]
    [Trait("Unit - Application", "Create Category")]
    [MemberData(
        nameof(CreateCategoryDataGenerator.GetInvalidRequests),
        parameters: 25,
        MemberType = typeof(CreateCategoryDataGenerator))]
    public async void Should_Throw_Error_When_Request_IsInvalid(
        CreateCategoryRequest invalidRequest,
        string exceptionMessage)
    {
        var useCase = new UseCase.CreateCategory(
            fixture.GetMockRepository().Object,
            fixture.GetMockUnitOfWork().Object);

        var action = async () =>
            await useCase.Handle(invalidRequest, CancellationToken.None);

        await action.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }


}
