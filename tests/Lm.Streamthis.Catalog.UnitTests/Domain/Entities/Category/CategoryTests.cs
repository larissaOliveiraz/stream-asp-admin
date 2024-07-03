using Lm.Streamthis.Catalog.Domain.Exceptions;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

public class CategoryTests
{
    [Fact(DisplayName = nameof(Should_Initialize_With_Valid_Values))]
    [Trait("Domain", "Category")]
    public void Should_Initialize_With_Valid_Values()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntities.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(category.Name, validData.Name);
        Assert.Equal(category.Description, validData.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(Should_Initialize_With_Specified_IsActive_Value))]
    [Trait("Domain", "Category")]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Initialize_With_Specified_IsActive_Value(bool isActive)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntities.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(category.Name, validData.Name);
        Assert.Equal(category.Description, validData.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(category.IsActive, isActive);
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Name_IsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Error_When_Name_IsEmpty(string? name)
    {
        var exception = Assert.Throws<EntityValidationException>(Action);
        Assert.Equal("Name should not be null or empty.", exception.Message);
        return;

        void Action() => 
            new DomainEntities.Category(name!, "category description");
    }
    
    [Fact(DisplayName = nameof(Should_Throw_Error_When_Description_IsNull))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Description_IsNull()
    {
        var exception = Assert.Throws<EntityValidationException>(Action);
        Assert.Equal("Description should not be null.", exception.Message);
        return;

        void Action() => 
            new DomainEntities.Category("category name", null!);
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Name_Has_Less_Than_3_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Name_Has_Less_Than_3_Characters()
    {
        var exception = Assert.Throws<EntityValidationException>(Action);
        Assert.Equal("Name should be at least three characters long.", exception.Message);
        return;
        
        void Action() => 
            new DomainEntities.Category("ca", "category description");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Name_Has_More_Than_255_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Name_Has_More_Than_255_Characters()
    {
        var invalidName = string.Join("", Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        
        var exception = Assert.Throws<EntityValidationException>(Action);
        Assert.Equal("Name should be equal or less than 255 characters long.", exception.Message);
        return;

        void Action() => 
            new DomainEntities.Category(invalidName, "category description");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Description_Has_More_Than_10000_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Description_Has_More_Than_10000_Characters()
    {
        var invalidDescription = string.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());

        var exception = Assert.Throws<EntityValidationException>(Action);
        Assert.Equal("Description should be equal or less than 10.000 characters long.", exception.Message);
        return;
        
        void Action() => 
            new DomainEntities.Category("category name", invalidDescription);
    }
}