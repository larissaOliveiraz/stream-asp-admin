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
        Action action = () => 
            new DomainEntities.Category(name!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be null or empty.", exception.Message);
    }
    
    [Theory(DisplayName = nameof(Should_Throw_Error_When_Description_IsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Error_When_Description_IsEmpty(string? name)
    {
        Action action = () => 
            new DomainEntities.Category(name!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be null or empty.", exception.Message);
    }
}