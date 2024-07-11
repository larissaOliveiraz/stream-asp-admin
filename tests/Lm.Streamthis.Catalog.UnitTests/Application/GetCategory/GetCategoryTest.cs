using FluentAssertions;
using Moq;
using Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;
using UseCases = Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryTest(GetCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Get_Category))]
    [Trait("Application", "Get Category")]
    public async void Should_Get_Category()
    {
        var repositoryMock = fixture.GetMockRepository();
        var category = fixture.GetValidCategory();

        repositoryMock
            .Setup(x => 
                x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var request = new GetCategoryRequest(category.Id);

        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var response = await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        response.Should().NotBeNull();
        response.Name.Should().Be(category.Name);
        response.Description.Should().Be(category.Description);
        response.IsActive.Should().Be(category.IsActive);
        response.Id.Should().Be(category.Id);
        response.CreatedAt.Should().Be(category.CreatedAt);
    }
}