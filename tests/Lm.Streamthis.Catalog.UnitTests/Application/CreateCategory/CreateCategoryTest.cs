using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Interfaces;
using UseCases = Lm.Streamthis.Catalog.Application.UseCases.Category.CreateCategory;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.Repositories;
using Moq;

namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(Should_Create_Category))]
    [Trait("Application", "Create Category")]
    public async void Should_Create_Category()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new UseCases.CreateCategoryInput("category name", "category description");

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
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}