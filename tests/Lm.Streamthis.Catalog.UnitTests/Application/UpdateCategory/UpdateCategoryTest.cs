using Moq;
using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.Domain.Entities;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest(UpdateCategoryFixture fixture)
{
    [Theory(DisplayName = nameof(Should_Update_Category))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetValidCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category(Category category, UpdateCategoryRequest request)
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
        response.IsActive.Should().Be(request.IsActive);
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
}