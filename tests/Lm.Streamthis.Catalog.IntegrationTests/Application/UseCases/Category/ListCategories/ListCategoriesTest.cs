using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using Lm.Streamthis.Catalog.Infra.Repositories;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

[Collection(nameof(ListCategoriesFixture))]
public class ListCategoriesTest(ListCategoriesFixture fixture)
{
    [Fact(DisplayName = nameof(Should_List_Categories))]
    [Trait("Integration - Application", "List Categories")]
    public async void Should_List_Categories()
    {
        var dbContext = fixture.CreateDbContext();
        var categories = fixture.GetCategoriesList(20);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var request = new ListCategoriesRequest(1, 20);

        var useCase = new UseCase.ListCategories(repository);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCount(categories.Count);
        response.Total.Should().Be(categories.Count);
        response.Page.Should().Be(request.Page);
        response.PerPage.Should().Be(request.PerPage);
        response.Items.ForEach(item =>
        {
            var category = categories.Find(x => x.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(category!.Name);
            item.Description.Should().Be(category.Description);
            item.IsActive.Should().Be(category.IsActive);
            item.CreatedAt.Should().Be(category.CreatedAt);
        });
    }
}
