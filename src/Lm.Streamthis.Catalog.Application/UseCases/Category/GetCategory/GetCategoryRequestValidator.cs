using FluentValidation;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryRequestValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryRequestValidator() =>
        RuleFor(x => x.Id).NotEmpty();
}