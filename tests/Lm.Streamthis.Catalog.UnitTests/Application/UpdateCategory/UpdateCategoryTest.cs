using Moq;
using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest(UpdateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Update_Category))]
    [Trait("Application", "Update Category")]
    public async void Should_Update_Category()
    {
        var repositoryMock = fixture.GetMockRepository();
        var unitOfWork = fixture.GetMockUnitOfWork();

        var validCategory = fixture.GetValidCategory();

        repositoryMock
            .Setup(x =>
                x.Get(validCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validCategory);

        var request = new UpdateCategoryRequest(
            validCategory.Id,
            fixture.GetValidCategoryName(),
            fixture.GetValidCategoryDescription(),
            !validCategory.IsActive
        );

        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        repositoryMock.Verify(repository => 
            repository.Get(validCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repository => 
            repository.Update(validCategory, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(uow => 
            uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}