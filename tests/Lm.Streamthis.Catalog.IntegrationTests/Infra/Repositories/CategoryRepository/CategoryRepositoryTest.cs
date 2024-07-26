using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using System.Reflection;
using Repository = Lm.Streamthis.Catalog.Infra.Repositories;

namespace Lm.Streamthis.Catalog.IntegrationTests.Infra.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryTest(CategoryRepositoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Insert_Category))]
    [Trait("Infra", "Category Repository")]
    public async Task Should_Insert_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var validCategory = fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(validCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var insertedCategory = await (fixture.CreateDbContext(true)).Categories().FindAsync(validCategory.Id);

        insertedCategory.Should().NotBeNull();
        insertedCategory.Name.Should().Be(validCategory.Name);
        insertedCategory.Description.Should().Be(validCategory.Description);
        insertedCategory.IsActive.Should().Be(validCategory.IsActive);
        insertedCategory.CreatedAt.Should().Be(validCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Get_Category))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Get_Category()
    {
        var dbContext = fixture.CreateDbContext();

        var validCategory = fixture.GetValidCategory();
        var validCategoryList = fixture.GetValidCategoryList(15);
        validCategoryList.Add(validCategory);

        await dbContext.AddRangeAsync(validCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(fixture.CreateDbContext(true));

        var selectedCategory = await categoryRepository.Get(validCategory.Id, CancellationToken.None);

        selectedCategory.Should().NotBeNull();
        selectedCategory.Id.Should().Be(validCategory.Id);
        selectedCategory.Name.Should().Be(validCategory.Name);
        selectedCategory.Description.Should().Be(validCategory.Description);
        selectedCategory.IsActive.Should().Be(validCategory.IsActive);
        selectedCategory.CreatedAt.Should().Be(validCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Category_Not_Found))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Throw_Exception_When_Category_Not_Found()
    {
        var dbContext = fixture.CreateDbContext();
        var randomId = Guid.NewGuid();

        await dbContext.AddRangeAsync(fixture.GetValidCategoryList(15));
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var action = async () =>
            await categoryRepository.Get(randomId, CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category with id '{randomId}' was not found.");
    }

    [Fact(DisplayName = nameof(Should_Update_Category))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Update_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var categoryList = fixture.GetValidCategoryList(15);
        var category = fixture.GetValidCategory();
        categoryList.Add(category);

        await dbContext.AddRangeAsync(categoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var newCategory = fixture.GetValidCategory();

        category.Update(newCategory.Name, newCategory.Description);

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        await categoryRepository.Update(category, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var updatedCategory = await (fixture.CreateDbContext(true)).Categories().FindAsync(category.Id);

        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(newCategory.Name);
        updatedCategory.Description.Should().Be(newCategory.Description);
    }

    [Fact(DisplayName = nameof(Should_Delete_Category))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Delete_Category()
    {
        var dbContext = fixture.CreateDbContext();
        var categoryList = fixture.GetValidCategoryList(15);
        var category = fixture.GetValidCategory();
        categoryList.Add(category);

        await dbContext.AddRangeAsync(categoryList);
        await dbContext.SaveChangesAsync();

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        await categoryRepository.Delete(category, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var deletedCategory = await fixture.CreateDbContext().Categories().FindAsync(category.Id);

        deletedCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(Should_Search_Category_Return_List_And_Total))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Search_Category_Return_List_And_Total()
    {
        var dbContext = fixture.CreateDbContext();
        var categoryList = fixture.GetValidCategoryList(15);
        var category = fixture.GetValidCategory();
        categoryList.Add(category);

        var searchRequest = new SearchRequest(1, 20, "", "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(categoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchedCategory = await categoryRepository.Search(searchRequest, CancellationToken.None);

        searchedCategory.Should().NotBeNull();
        searchedCategory.CurrentPage.Should().Be(searchRequest.Page);
        searchedCategory.PerPage.Should().Be(searchRequest.PerPage);
        searchedCategory.Total.Should().Be(categoryList.Count);
        searchedCategory.Items.Should().HaveCount(categoryList.Count);
        searchedCategory.Items.ForEach(item =>
        {
            var category = categoryList.Find(category => category.Id == item.Id);
            category.Should().NotBeNull();
            category.Name.Should().Be(item.Name);
            category.Description.Should().Be(item.Description);
            category.IsActive.Should().Be(item.IsActive);
            category.CreatedAt.Should().Be(item.CreatedAt);
        });
    }

    [Fact(DisplayName = nameof(Should_Return_Empty_List_When_Search_HasNoItems))]
    [Trait("Infra", "Category Repository")]
    public async void Should_Return_Empty_List_When_Search_HasNoItems()
    {
        var dbContext = fixture.CreateDbContext();

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchRequest = new SearchRequest(1, 20, "", "", SearchOrder.Asc);

        var searchResponse = await categoryRepository.Search(searchRequest, CancellationToken.None);

        searchResponse.Should().NotBeNull();
        searchResponse.Items.Should().NotBeNull();
        searchResponse.Items.Should().HaveCount(0);
        searchResponse.Total.Should().Be(0);
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
    }

    [Theory(DisplayName = nameof(Should_Return_Search_Results_Paginated))]
    [Trait("Infra", "Category Repository")]
    [InlineData(20, 1, 10, 10)]
    [InlineData(20, 2, 10, 10)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async void Should_Return_Search_Results_Paginated(
        int categoriesAmount, int currentPage, int perPage, int expectedItemsInCurrentPage)
    {
        var dbContext = fixture.CreateDbContext();
        var categoryList = fixture.GetValidCategoryList(categoriesAmount);

        await dbContext.AddRangeAsync(categoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new SearchRequest(currentPage, perPage, "", "", SearchOrder.Asc);

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchResponse = await categoryRepository.Search(searchRequest, CancellationToken.None);

        searchResponse.Should().NotBeNull();
        searchResponse.Items.Should().HaveCount(expectedItemsInCurrentPage);
        searchResponse.Total.Should().Be(categoriesAmount);
        searchResponse.CurrentPage.Should().Be(currentPage);
        searchResponse.PerPage.Should().Be(perPage);
        searchResponse.Items.ForEach(async item =>
        {
            var category = await dbContext.Categories().FindAsync(item.Id);
            category.Should().NotBeNull();
            item.Name.Should().Be(category.Name);
            item.Description.Should().Be(category.Description);
            item.IsActive.Should().Be(category.IsActive);
            item.CreatedAt.Should().Be(category.CreatedAt);
        });
    }

    [Theory(DisplayName = nameof(Should_Search_By_Text))]
    [Trait("Infra", "Category Repository")]
    [InlineData("Romance", 1, 5, 2, 2)]
    [InlineData("Fantasy", 1, 2, 2, 3)]
    [InlineData("Fantasy", 2, 5, 0, 3)]
    [InlineData("Fantasy", 2, 2, 1, 3)]
    [InlineData("Random", 1, 5, 0, 0)]
    public async void Should_Search_By_Text(
        string search, int currentPage, int perPage, int expectedItemsInCurrentPage, int expectedTotalItems)
    {
        var dbContext = fixture.CreateDbContext();
        var categoriesList = fixture.GetCategoriesListWithName(
            ["Fantasy", "Romance", "Sci-Fi", "Horror", "Drama", "Fantasy Romance", "Comedy", "Historical Fantasy"]);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync();

        var searchRequest = new SearchRequest(currentPage, perPage, search, "", SearchOrder.Asc);

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchedCategories = await categoryRepository.Search(searchRequest, CancellationToken.None);

        searchedCategories.Should().NotBeNull();
        searchedCategories.CurrentPage.Should().Be(currentPage);
        searchedCategories.PerPage.Should().Be(perPage);
        searchedCategories.Total.Should().Be(expectedTotalItems);
        searchedCategories.Items.Should().NotBeNull();
        searchedCategories.Items.Should().HaveCount(expectedItemsInCurrentPage);
        searchedCategories.Items.ForEach(async item =>
        {
            var category = await dbContext.Categories().FindAsync(item.Id);
            category.Should().NotBeNull();
            item.Name.Should().Be(category.Name);
            item.Description.Should().Be(category.Description);
            item.IsActive.Should().Be(category.IsActive);
            item.CreatedAt.Should().Be(category.CreatedAt);
        });
    }

    [Theory(DisplayName = nameof(Should_Return_Search_Results_Ordered))]
    [Trait("Infra", "Category Repository")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    [InlineData("id", SearchOrder.Asc)]
    [InlineData("id", SearchOrder.Desc)]
    [InlineData("createdAt", SearchOrder.Asc)]
    [InlineData("createdAt", SearchOrder.Desc)]
    [InlineData("", SearchOrder.Asc)]
    public async void Should_Return_Search_Results_Ordered(string orderBy, SearchOrder order)
    {
        var dbContext = fixture.CreateDbContext();
        var categoriesList = fixture.GetValidCategoryList(20);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync();

        var searchRequest = new SearchRequest(1, 20, "", orderBy, order);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchedCategories = await categoryRepository.Search(searchRequest, CancellationToken.None);

        searchedCategories.Should().NotBeNull();
        var orderFunctions = new Dictionary<(string, SearchOrder), Action<IEnumerable<Category>>>
        {
            { ("name", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Name) },
            { ("name", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.Name) },
            { ("id", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Id) },
            { ("id", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.Id) },
            { ("createdAt", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.CreatedAt) },
            { ("createdAt", SearchOrder.Desc), categories => categories.Should().BeInDescendingOrder(x => x.CreatedAt) },
            { ("", SearchOrder.Asc), categories => categories.Should().BeInAscendingOrder(x => x.Name) },
        };
        orderFunctions[(orderBy, order)](searchedCategories.Items);
    }
}
