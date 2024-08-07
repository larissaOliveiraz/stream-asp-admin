using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;
using Moq;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryFixture))]
public class DeleteCategoryTest(DeleteCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Delete_Category))]
    [Trait("Unit - Application", "Delete Category")]
    public async void Should_Delete_Category()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        var category = fixture.GetCategory();

        repositoryMock
            .Setup(x =>
                x.Get(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var validRequest = new DeleteCategoryRequest(category.Id);

        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWork.Object);
        await useCase.Handle(validRequest, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Get(category.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repository =>
            repository.Delete(category, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Unit - Application", "Delete Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        var randomId = Guid.NewGuid();

        repositoryMock
            .Setup(x =>
                x.Get(randomId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new NotFoundException($"Category with id '{randomId}' was not found."));

        var request = new DeleteCategoryRequest(randomId);

        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWork.Object);

        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository =>
            repository.Get(randomId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
