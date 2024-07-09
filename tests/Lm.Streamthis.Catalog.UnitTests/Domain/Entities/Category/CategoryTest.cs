using FluentAssertions;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Entities.Category;

[Collection(nameof(CategoryFixture))]
public class CategoryTest(CategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Initialize_With_Valid_Values))]
    [Trait("Domain", "Category")]
    public void Should_Initialize_With_Valid_Values()
    {
        var validCategory = fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntities.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Should_Initialize_With_Specified_IsActive_Value))]
    [Trait("Domain", "Category")]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Initialize_With_Specified_IsActive_Value(bool isActive)
    {
        var validCategory = fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntities.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Name_IsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Error_When_Name_IsEmpty(string? name)
    {
        var validCategory = fixture.GetValidCategory();
        
        var action = () =>
            new DomainEntities.Category(name!, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
    
    [Fact(DisplayName = nameof(Should_Throw_Error_When_Description_IsNull))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Description_IsNull()
    {
        var validCategory = fixture.GetValidCategory();
        
        var action = () => 
            new DomainEntities.Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null.");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Name_Has_Less_Than_3_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Name_Has_Less_Than_3_Characters()
    {
        var validCategory = fixture.GetValidCategory();
        
        var action = () => 
            new DomainEntities.Category("ca", validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not have less than 3 characters.");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Name_Has_More_Than_255_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Name_Has_More_Than_255_Characters()
    {
        var invalidName = string.Join("", Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        var validCategory = fixture.GetValidCategory();
        
        var action = () => 
            new DomainEntities.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not have more than 255 characters.");
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Description_Has_More_Than_10000_Characters))]
    [Trait("Domain", "Category")]
    public void Should_Throw_Error_When_Description_Has_More_Than_10000_Characters()
    {
        var invalidDescription = string.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());
        var validCategory = fixture.GetValidCategory();

        var action = () => 
            new DomainEntities.Category(validCategory.Name, invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not have more than 10000 characters.");
    }
    
    [Fact(DisplayName = nameof(Should_Activate))]
    [Trait("Domain", "Category")]
    public void Should_Activate()
    {
        var validCategory = fixture.GetValidCategory();

        var category = 
            new DomainEntities.Category(validCategory.Name, validCategory.Description, false);

        category.Activate();
        
        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Should_Deactivate))]
    [Trait("Domain", "Category")]
    public void Should_Deactivate()
    {
        var validCategory = fixture.GetValidCategory();

        var category = 
            new DomainEntities.Category(validCategory.Name, validCategory.Description);

        category.Deactivate();
        
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Should_Update_Name_And_Description))]
    [Trait("Domain", "Category")]
    public void Should_Update_Name_And_Description()
    {
        var category = fixture.GetValidCategory();
        var newCategory = fixture.GetValidCategory();

        category.Update(newCategory.Name, newCategory.Description);
        
        category.Name.Should().Be(newCategory.Name);
        category.Description.Should().Be(newCategory.Description);
    }
    
    [Fact(DisplayName = nameof(Should_Update_Name))]
    [Trait("Domain", "Category")]
    public void Should_Update_Name()
    {
        var category = fixture.GetValidCategory();
        var newName = fixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);
        
        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }
}