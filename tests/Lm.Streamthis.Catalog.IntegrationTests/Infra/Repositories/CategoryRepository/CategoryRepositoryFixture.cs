﻿using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Infra;
using Lm.Streamthis.Catalog.IntegrationTests.Common;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.IntegrationTests.Infra.Repositories.CategoryRepository;

public class CategoryRepositoryFixture : BaseFixture
{
    private string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    private string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    private static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public Category GetValidCategory() =>
        new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());


    public List<Category> GetValidCategoryList(int length) =>
        Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();

    public List<Category> GetCategoriesListWithName(List<string> names) =>
        names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
}

[CollectionDefinition(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryFixtureCollection : ICollectionFixture<CategoryRepositoryFixture>
{
}
