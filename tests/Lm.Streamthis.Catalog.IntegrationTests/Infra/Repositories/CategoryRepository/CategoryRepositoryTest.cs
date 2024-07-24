using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
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
}