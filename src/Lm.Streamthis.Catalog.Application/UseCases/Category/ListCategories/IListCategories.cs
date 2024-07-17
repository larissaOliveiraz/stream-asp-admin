using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

public interface IListCategories : IRequestHandler<ListCategoriesRequest, ListCategoriesResponse>
{
}