using Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;
using Lm.Streamthis.Catalog.Infra.Repositories;
using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryTest(GetCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Get_Category))]
    [Trait("Integration - Application", "Get Category")]
    public async void Should_Get_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var validCategory = fixture.GetCategory();

        dbContext.Categories().Add(validCategory);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);

        var request = new GetCategoryRequest(validCategory.Id);
        var useCase = new UseCase.GetCategory(repository);

        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(validCategory.Id);
        response.Name.Should().Be(validCategory.Name);
        response.Description.Should().Be(validCategory.Description);
        response.IsActive.Should().Be(validCategory.IsActive);
        response.CreatedAt.Should().Be(validCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Integration - Application", "Get Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var dbContext = fixture.CreateDbContext();

        var repository = new CategoryRepository(dbContext);

        var request = new GetCategoryRequest(Guid.NewGuid());
        var useCase = new UseCase.GetCategory(repository);

        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category with id '{request.Id}' was not found.");
    }
}
