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
            
            var validRequest = new UpdateCategoryRequest(
                validCategory.Id, 
                fixture.GetValidCategoryName(),
                fixture.GetValidCategoryDescription(), 
                fixture.GetRandomBoolean());

            yield return [validCategory, validRequest];
        }
    }
}