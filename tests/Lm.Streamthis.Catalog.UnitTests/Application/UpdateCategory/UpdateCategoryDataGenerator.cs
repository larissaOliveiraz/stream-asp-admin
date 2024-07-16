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
    
    public static IEnumerable<object[]> GetInvalidRequests(int times)
    {
        var fixture = new UpdateCategoryFixture();

        var invalidRequestList = new List<object[]>();
        const int totalInvalidUseCases = 5;
    
        for (var i = 0; i < times; i++)
        {
            switch (i % totalInvalidUseCases)
            {
                case 0:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestShortName(), 
                        "Name should not have less than 3 characters."]);
                    break;
                case 1:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestLongName(), 
                        "Name should not have more than 255 characters."]);
                    break;
                case 2:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestLongDescription(), 
                        "Description should not have more than 10000 characters."]);
                    break;
            }    
        }
        
        return invalidRequestList;
    }
}