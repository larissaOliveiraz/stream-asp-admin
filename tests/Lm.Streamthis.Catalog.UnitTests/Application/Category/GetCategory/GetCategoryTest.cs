using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;
using Moq;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.GetCategory;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryTest(GetCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Get_Category))]
    [Trait("Unit - Application", "Get Category")]
    public async void Should_Get_Category()
    {
        var repositoryMock = fixture.GetMockRepository();
        var category = fixture.GetValidCategory();

        repositoryMock
            .Setup(x =>
                x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var request = new GetCategoryRequest(category.Id);

        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var response = await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        response.Should().NotBeNull();
        response.Name.Should().Be(category.Name);
        response.Description.Should().Be(category.Description);
        response.IsActive.Should().Be(category.IsActive);
        response.Id.Should().Be(category.Id);
        response.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Unit - Application", "Get Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var repositoryMock = fixture.GetMockRepository();
        var randomId = Guid.NewGuid();

        repositoryMock
            .Setup(x =>
                x.Get(randomId, CancellationToken.None))
            .ThrowsAsync(new NotFoundException($"Category with id '{randomId}' was not found."));

        var request = new GetCategoryRequest(randomId);

        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }
}
