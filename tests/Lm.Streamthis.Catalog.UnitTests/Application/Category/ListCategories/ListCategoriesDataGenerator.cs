using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.ListCategories;

public class ListCategoriesDataGenerator
{
    public static IEnumerable<object[]> GetValidRequestsWithoutAnyParameters(int? times = 12)
    {
        var fixture = new ListCategoriesFixture();
        var request = fixture.GetRequest();
        const int totalCases = 6;

        for (var i = 0; i < times; i++)
        {
            yield return (i % totalCases) switch
            {
                0 => [new ListCategoriesRequest()],
                1 => [new ListCategoriesRequest(request.Page)],
                2 => [new ListCategoriesRequest(request.Page, request.PerPage)],
                3 => [new ListCategoriesRequest(request.Page, request.PerPage, request.Search)],
                4 => [new ListCategoriesRequest(request.Page, request.PerPage, request.Search, request.Sort)],
                5 => [request],
                _ => [new ListCategoriesRequest()]
            };
        }
    }
}