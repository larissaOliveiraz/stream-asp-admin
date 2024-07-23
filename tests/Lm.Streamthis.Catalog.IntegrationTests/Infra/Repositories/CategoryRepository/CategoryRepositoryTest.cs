﻿using FluentAssertions;
using Lm.Streamthis.Catalog.Infra;
using Repository = Lm.Streamthis.Catalog.Infra.Repositories;

namespace Lm.Streamthis.Catalog.IntegrationTests.Infra.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryTest(CategoryRepositoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Insert_Category))]
    [Trait("Infra.Data", "Category Repository")]
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
}