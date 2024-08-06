namespace Lm.Streamthis.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryFixture();

        for (int i = 0; i < times; i++)
        {
            var category = fixture.GetCategory();
            var request = fixture.GetRequest(category.Id);

            yield return [category, request];
        }
    }

    public static IEnumerable<object[]> GetInvalidRequests(int times = 10)
    {
        var fixture = new UpdateCategoryFixture();

        var invalidRequestsList = new List<object[]>();
        var totalInvalidUseCases = 3;

        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidUseCases)
            {
                case 0:
                    invalidRequestsList.Add([
                        fixture.GetInvalidRequestShortName(),
                        "Name should not have less than 3 characters."]);
                    break;
                case 1:
                    invalidRequestsList.Add([
                        fixture.GetInvalidRequestLongName(),
                        "Name should not have more than 255 characters."]);
                    break;
                case 2:
                    invalidRequestsList.Add([
                        fixture.GetInvalidRequestLongDescription(),
                        "Description should not have more than 10000 characters."]);
                    break;
            }

        }

        return invalidRequestsList;
    }
}
