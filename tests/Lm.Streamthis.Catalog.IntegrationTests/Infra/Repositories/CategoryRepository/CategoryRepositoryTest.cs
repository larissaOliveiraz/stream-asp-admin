using FluentAssertions;
using Lm.Streamthis.Catalog.Application.Exceptions;
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

        var insertedCategory = await dbContext.Categories().FindAsync(validCategory.Id);

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

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var selectedCategory = await categoryRepository.Get(validCategory.Id, CancellationToken.None);

        selectedCategory.Should().NotBeNull();
        selectedCategory.Id.Should().Be(validCategory.Id);
        selectedCategory.Name.Should().Be(validCategory.Name);
        selectedCategory.Description.Should().Be(validCategory.Description);
        selectedCategory.IsActive.Should().Be(validCategory.IsActive);
        selectedCategory.CreatedAt.Should().Be(validCategory.CreatedAt);
    }

    [Fact(DisplayName =nameof(Should_Throw_Exception_When_Category_Not_Found))]
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
}