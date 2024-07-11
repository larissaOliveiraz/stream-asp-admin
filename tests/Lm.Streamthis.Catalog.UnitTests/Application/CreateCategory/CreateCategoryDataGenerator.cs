namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidRequests(int times)
    {
        var fixture = new CreateCategoryFixture();

        var invalidRequestList = new List<object[]>();
        const int totalInvalidUseCases = 5;
    
        for (var i = 0; i < times; i++)
        {
            switch (i % totalInvalidUseCases)
            {
                case 0:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestNullName(), 
                        "Name should not be null or empty."]);
                    break;
                case 1:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestShortName(), 
                        "Name should not have less than 3 characters."]);
                    break;
                case 2:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestLongName(), 
                        "Name should not have more than 255 characters."]);
                    break;
                case 3:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestNullDescription(), 
                        "Description should not be null."]);
                    break;
                case 4:
                    invalidRequestList.Add([
                        fixture.GetInvalidRequestLongDescription(), 
                        "Description should not have more than 10000 characters."]);
                    break;
            }    
        }
        
        return invalidRequestList;
    }
}