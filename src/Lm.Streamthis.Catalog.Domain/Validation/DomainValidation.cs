using Lm.Streamthis.Catalog.Domain.Exceptions;

namespace Lm.Streamthis.Catalog.Domain.Validation;

public class DomainValidation
{
    public static void NotNull(object value, string field)
    {
        if (value is null)
            throw new EntityValidationException($"{field} should not be null.");
    }

    public static void NotNullOrEmpty(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EntityValidationException($"{field} should not be null or empty.");
    }

    public static void MinLength(string value, int minLength, string field)
    {
        if (value.Length < minLength)
            throw new EntityValidationException($"{field} should not have less than {minLength} characters.");
    }

    public static void MaxLength(string value, int maxLength, string field)
    {
        if (value.Length > maxLength)
            throw new EntityValidationException($"{field} should not have more than {maxLength} characters.");
    }
}