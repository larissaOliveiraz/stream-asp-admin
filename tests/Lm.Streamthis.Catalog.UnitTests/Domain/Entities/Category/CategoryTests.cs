using FluentAssertions;
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

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
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

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Name_IsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Error_When_Name_IsEmpty(string? name)
    {
        var action = () =>
            new DomainEntities.Category(name!, "category description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
    
    [Fact(DisplayName = nameof(Should_Throw_Error_When_Description_IsNull))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Description_IsNull()
    {
        var action = () => 
            new DomainEntities.Category("category name", null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null.");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Name_Has_Less_Than_3_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Name_Has_Less_Than_3_Characters()
    {
        var action = () => 
            new DomainEntities.Category("ca", "category description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least three characters long.");
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

        var action = () => 
            new DomainEntities.Category("category name", invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be equal or less than 10.000 characters long.");
    }
    
    [Fact(DisplayName = nameof(Should_Activate))]
    [Trait("Domain", "Category")]
    public void Should_Activate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntities.Category(validData.Name, validData.Description, false);

        category.Activate();
        
        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Should_Deactivate))]
    [Trait("Domain", "Category")]
    public void Should_Deactivate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntities.Category(validData.Name, validData.Description);

        category.Deactivate();
        
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Should_Update_Name_And_Description))]
    [Trait("Domain", "Category")]
    public void Should_Update_Name_And_Description()
    {
        var category = new DomainEntities.Category("category name", "category description");
        var newCategory = new { Name = "new name", Description = "new description" };

        category.Update(newCategory.Name, newCategory.Description);
        
        category.Name.Should().Be(newCategory.Name);
        category.Description.Should().Be(newCategory.Description);
    }
    
    [Fact(DisplayName = nameof(Should_Update_Name))]
    [Trait("Domain", "Category")]
    public void Should_Update_Name()
    {
        var category = new DomainEntities.Category("category name", "category description");
        var newCategory = new { Name = "new name" };

        category.Update(newCategory.Name);
        
        category.Name.Should().Be(newCategory.Name);
        category.Description.Should().Be("category description");
    }
}