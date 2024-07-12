using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryRequestValidatorTest(GetCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Validate_Request))]
    [Trait("Application", "Get Category Request Validator")]
    public void Should_Validate_Request()
    {
        var validId = Guid.NewGuid();
        var validRequest = new GetCategoryRequest(validId);

        var validator = new GetCategoryRequestValidator();

        var result = validator.Validate(validRequest);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Request_IsInvalid))]
    [Trait("Application", "Get Category Request Validator")]
    public void Should_Throw_Exception_When_Request_IsInvalid()
    {
        var invalidId = Guid.Empty;
        var invalidRequest = new GetCategoryRequest(invalidId);

        var validator = new GetCategoryRequestValidator();

        var result = validator.Validate(invalidRequest);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
    }
}