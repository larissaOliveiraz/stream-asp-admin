namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int? times = 10)
    {
        var fixture = new UpdateCategoryFixture();

        for (int i = 0; i < times; i++)
        {
            var category = fixture.GetValidCategory();
            var request = fixture.GetRequest(category.Id);

            yield return [category, request];
        }
    }
}
