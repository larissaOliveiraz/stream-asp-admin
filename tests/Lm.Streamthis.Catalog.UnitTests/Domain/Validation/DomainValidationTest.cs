using Bogus;
using FluentAssertions;
using Lm.Streamthis.Catalog.Domain.Exceptions;
using Lm.Streamthis.Catalog.Domain.Validation;

namespace Lm.Streamthis.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; } = new();
    
    [Fact(DisplayName = nameof(Should_Not_Be_Null))]
    [Trait("Domain", "Validation")]
    public void Should_Not_Be_Null()
    {
        var value = Faker.Commerce.ProductName();

        var action = () =>
            DomainValidation.NotNull(value, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(Should_Throw_Error_When_Value_IsNull))]
    [Trait("Domain", "Validation")]
    public void Should_Throw_Error_When_Value_IsNull()
    {
        object? invalidValue = null;

        var action = () =>
            DomainValidation.NotNull(invalidValue!, "Field");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field should not be null.");
    }

    [Fact(DisplayName = nameof(Should_Not_Be_Null_Or_Empty))]
    [Trait("Domain", "Validation")]
    public void Should_Not_Be_Null_Or_Empty()
    {
        var value = Faker.Commerce.ProductName();

        var action = () =>
            DomainValidation.NotNullOrEmpty(value, "Field");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Value_IsNull_OrEmpty))]
    [Trait("Domain", "Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Throw_Error_When_Value_IsNull_OrEmpty(string invalidValue)
    {
        var action = () =>
            DomainValidation.NotNullOrEmpty(invalidValue, "Field");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field should not be null or empty.");
    }

    [Fact(DisplayName = nameof(Should_Not_Have_Less_Characters_Than_Minimum))]
    [Trait("Domain", "Validation")]
    public void Should_Not_Have_Less_Characters_Than_Minimum()
    {
        var action = () =>
            DomainValidation.MinLength("1234", 3, "Field");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Value_IsLess_Than_MinLength))]
    [Trait("Domain", "Validation")]
    [InlineData("12", 3)]
    public void Should_Throw_Error_When_Value_IsLess_Than_MinLength(string value, int minLength)
    { 
        var action = () =>
            DomainValidation.MinLength(value, minLength, "Field");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field should not have less than {minLength} characters.");
    }

    [Fact(DisplayName = nameof(Should_Not_Have_More_Characters_Than_MaxLength))]
    [Trait("Domain", "Validation")]
    public void Should_Not_Have_More_Characters_Than_MaxLength()
    {
        var action = () =>
            DomainValidation.MaxLength("1234", 5, "Field");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(Should_Throw_Error_When_Value_IsGreater_Than_MaxLength))]
    [Trait("Domain", "Validation")]
    [InlineData("123456", 5)]
    public void Should_Throw_Error_When_Value_IsGreater_Than_MaxLength(string value, int maxLength)
    {
        var action = () =>
            DomainValidation.MaxLength(value, maxLength, "Field");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field should not have more than {maxLength} characters.");
    }
}