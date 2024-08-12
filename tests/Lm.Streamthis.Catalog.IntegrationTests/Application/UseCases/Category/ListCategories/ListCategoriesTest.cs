using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Lm.Streamthis.Catalog.Infra.Repositories;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
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

    [Fact(DisplayName = nameof(Should_Return_Empty_List_When_Search_HasNoItems))]
    [Trait("Integration - Application", "List Categories")]
    public async void Should_Return_Empty_List_When_Search_HasNoItems()
    {
        var dbContext = fixture.CreateDbContext();

        var repository = new CategoryRepository(dbContext);
        var request = new ListCategoriesRequest();

        var useCase = new UseCase.ListCategories(repository);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCount(0);
        response.Total.Should().Be(0);
        response.Page.Should().Be(request.Page);
        response.PerPage.Should().Be(request.PerPage);
    }

    [Theory(DisplayName = nameof(Should_Return_Search_Results_Paginated))]
    [Trait("Integration - Application", "List Categories")]
    [InlineData(20, 1, 10, 10)]
    [InlineData(20, 2, 10, 10)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async void Should_Return_Search_Results_Paginated(
        int categoriesAmount, int currentPage, int perPage, int expectedItemsInCurrentPage)
    {
        var dbContext = fixture.CreateDbContext();
        var categories = fixture.GetCategoriesList(categoriesAmount);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var request = new ListCategoriesRequest(currentPage, perPage);

        var useCase = new UseCase.ListCategories(repository);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().HaveCount(expectedItemsInCurrentPage);
        response.Total.Should().Be(categoriesAmount);
        response.Page.Should().Be(currentPage);
        response.PerPage.Should().Be(perPage);
        response.Items.ForEach(item =>
        {
            var category = categories.Find(x => x.Id == item.Id);
            item.Name.Should().Be(category!.Name);
            item.Description.Should().Be(category.Description);
            item.IsActive.Should().Be(category.IsActive);
            item.CreatedAt.Should().Be(category.CreatedAt);
        });
    }

    [Theory(DisplayName = nameof(Should_Search_By_Text))]
    [Trait("Integration - Application", "List Categories")]
    [InlineData("Romance", 1, 5, 2, 2)]
    [InlineData("Fantasy", 1, 2, 2, 3)]
    [InlineData("Fantasy", 2, 5, 0, 3)]
    [InlineData("Fantasy", 2, 2, 1, 3)]
    [InlineData("Random", 1, 5, 0, 0)]
    public async void Should_Search_By_Text(
        string search, int currentPage, int perPage, int expectedItemsInCurrentPage, int expectedTotalItems)
    {
        var dbContext = fixture.CreateDbContext();
        var categories = fixture.GetCategoriesListWithNames([
            "Fantasy", "Romance", "Sci-Fi", "Horror", "Drama", "Fantasy Romance", "Comedy", "Historical Fantasy"]);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var request = new ListCategoriesRequest(currentPage, perPage, search);

        var useCase = new UseCase.ListCategories(repository);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().HaveCount(expectedItemsInCurrentPage);
        response.Total.Should().Be(expectedTotalItems);
        response.Page.Should().Be(currentPage);
        response.PerPage.Should().Be(perPage);
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

    [Theory(DisplayName = nameof(Should_Return_Search_Results_Ordered))]
    [Trait("Integration - Application", "List Categories")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    [InlineData("id", SearchOrder.Asc)]
    [InlineData("id", SearchOrder.Desc)]
    [InlineData("createdAt", SearchOrder.Asc)]
    [InlineData("createdAt", SearchOrder.Desc)]
    [InlineData("", SearchOrder.Asc)]
    public async void Should_Return_Search_Results_Ordered(
        string orderBy, SearchOrder order)
    {
        var dbContext = fixture.CreateDbContext();
        var categories = fixture.GetCategoriesList(20);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var request = new ListCategoriesRequest(1, 20, "", orderBy, order);

        var useCase = new UseCase.ListCategories(repository);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        var orderedResponse = new Dictionary<(string, SearchOrder), Action<IEnumerable<CategoryResponse>>>
        {
            { ("name", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Name) },
            { ("name", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.Name) },
            { ("id", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Id) },
            { ("id", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.Id) },
            { ("createdAt", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.CreatedAt) },
            { ("createdAt", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.CreatedAt) },
            { ("", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Name) }
        };
        orderedResponse[(orderBy, order)](response.Items);
    }
}
