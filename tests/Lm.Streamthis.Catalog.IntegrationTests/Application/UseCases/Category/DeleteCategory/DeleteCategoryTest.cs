using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;
using Lm.Streamthis.Catalog.Infra;
using Lm.Streamthis.Catalog.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.DeleteCategory;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryFixture))]
public class DeleteCategoryTest(DeleteCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Delete_Category))]
    [Trait("Integration - Application", "Delete Category")]
    public async void Should_Delete_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var category = fixture.GetCategory();

        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new DeleteCategoryRequest(category.Id);

        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);
        await useCase.Handle(request, CancellationToken.None);

        var newDbContext = fixture.CreateDbContext(true);
        var deletedCategory = await newDbContext.Categories().FindAsync(category.Id);
        deletedCategory.Should().BeNull();
        newDbContext.Categories().Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_NotFound))]
    [Trait("Integration - Application", "Delete Category")]
    public async void Should_Throw_Exception_When_Category_NotFound()
    {
        var dbContext = fixture.CreateDbContext();
        var category = fixture.GetCategory();

        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var request = new DeleteCategoryRequest(Guid.NewGuid());

        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);
        var action = async () =>
            await useCase.Handle(request, CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category with id '{request.Id}' was not found.");

        var newDbContext = fixture.CreateDbContext(true);
        newDbContext.Categories().Should().HaveCount(1);
    }
}
