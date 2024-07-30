using FluentAssertions;
using InfraUOW = Lm.Streamthis.Catalog.Infra;

namespace Lm.Streamthis.Catalog.IntegrationTests.Infra.UnitOfWork;

[Collection(nameof(UnitOfWorkFixture))]
public class UnitOfWorkTest(UnitOfWorkFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Commit))]
    [Trait("Infra", "Unit of Work")]
    public async void Should_Commit()
    {
        var dbContext = fixture.CreateDbContext();
        var categoriesList = fixture.GetValidCategoryList(20);

        await dbContext.AddRangeAsync(categoriesList);

        var unitOfWork = new InfraUOW.UnitOfWork(dbContext);
        await unitOfWork.Commit(CancellationToken.None);

        var newDbContext = fixture.CreateDbContext(true);
        var savedCategories = newDbContext.Categories().ToList();

        savedCategories.Should().NotBeEmpty();
        savedCategories.Should().HaveCount(categoriesList.Count);
    }

    [Fact(DisplayName = nameof(Should_Rollback))]
    [Trait("Infra", "Unit of Work")]
    public async void Should_Rollback()
    {
        var dbContext = fixture.CreateDbContext();

        var unitOfWork = new InfraUOW.UnitOfWork(dbContext);

        var action = async () =>
            await unitOfWork.Rollback(CancellationToken.None);

        await action.Should().NotThrowAsync();
    }
}
