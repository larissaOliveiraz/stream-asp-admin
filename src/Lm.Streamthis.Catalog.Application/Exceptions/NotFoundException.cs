namespace Lm.Streamthis.Catalog.Application.Exceptions;

public class NotFoundException(string? message) : ApplicationException(message)
{
}