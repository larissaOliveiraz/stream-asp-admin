using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;
using Lm.Streamthis.Catalog.Infra.Repositories;
using Lm.Streamthis.Catalog.Infra;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryTest(UpdateCategoryFixture fixture)
{
    [Theory(DisplayName = nameof(Should_Update_Category))]
    [Trait("Application", "Update Category")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
        parameters: 3,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async void Should_Update_Category(
        DomainEntities.Category category, UpdateCategoryRequest request)
    {
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetValidCategoryList(10));
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var unitOfWork = new UnitOfWork(dbContext);

        var repository = new CategoryRepository(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(category.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        var updatedCategory = await fixture.CreateDbContext(true).Categories().FindAsync(response.Id);
        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(response.Name);
        updatedCategory.Description.Should().Be(response.Description);
        updatedCategory.IsActive.Should().Be((bool)response.IsActive);
        updatedCategory.CreatedAt.Should().Be(response.CreatedAt);
    }
}
