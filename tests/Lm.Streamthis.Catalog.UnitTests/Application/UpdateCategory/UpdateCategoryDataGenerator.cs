using Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Lm.Streamthis.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetValidCategoriesToUpdate(int? times = 10)
    {
        var fixture = new UpdateCategoryFixture();

        for (var i = 0; i < times; i++)
        {
            var validCategory = fixture.GetValidCategory();
            
            var validRequest = fixture.GetValidRequest(validCategory.Id);

            yield return [validCategory, validRequest];
        }
    }
}