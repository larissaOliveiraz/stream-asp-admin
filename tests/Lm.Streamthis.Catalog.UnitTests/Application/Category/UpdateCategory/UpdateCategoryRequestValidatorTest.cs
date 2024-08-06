using FluentAssertions;
using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryRequestValidatorTest(UpdateCategoryFixture fixture)
{
    [Fact(DisplayName = nameof(Should_Validate_Request))]
    [Trait("Unit - Application", "Update Category Request Validator")]
    public void Should_Validate_Request()
    {
        var request = fixture.GetRequest();

        var validator = new UpdateCategoryRequestValidator();

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(Should_Throw_Exception_When_Request_IsInvalid))]
    [Trait("Unit - Application", "Update Category Request Validator")]
    public void Should_Throw_Exception_When_Request_IsInvalid()
    {
        var requestWithInvalidId = fixture.GetRequest(Guid.Empty);

        var validator = new UpdateCategoryRequestValidator();

        var result = validator.Validate(requestWithInvalidId);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
    }
}
