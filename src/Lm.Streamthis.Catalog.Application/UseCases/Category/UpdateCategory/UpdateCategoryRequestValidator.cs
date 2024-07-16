using FluentValidation;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator() =>
        RuleFor(x => x.Id).NotEmpty();
}