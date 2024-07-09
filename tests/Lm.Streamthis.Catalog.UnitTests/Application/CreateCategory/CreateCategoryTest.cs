using FluentAssertions;
using UseCases = Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Domain.Entities;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryTest(CreateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Create_Category))]
    [Trait("Application", "Create Category")]
    public async void Should_Create_Category()
    {
        var repositoryMock = CreateCategoryFixture.GetMockRepository;
        var unitOfWorkMock = CreateCategoryFixture.GetMockUnitOfWork;
        
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
}