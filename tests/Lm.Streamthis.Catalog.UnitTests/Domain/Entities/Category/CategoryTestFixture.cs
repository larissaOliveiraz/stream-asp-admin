﻿using Lm.Streamthis.Catalog.UnitTests.Common;
using DomainEntity = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

public class CategoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
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
    
    public DomainEntity.Category GetValidCategory() =>
        new(GetValidCategoryName(),GetValidCategoryDescription());
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{
}