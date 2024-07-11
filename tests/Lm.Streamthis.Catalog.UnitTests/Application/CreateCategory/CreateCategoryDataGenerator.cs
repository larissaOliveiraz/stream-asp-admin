namespace Lm.Streamthis.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times)
    {
        var fixture = new CreateCategoryFixture();

        var invalidInputList = new List<object[]>();
        const int totalInvalidUseCases = 5;
    
        for (var i = 0; i < times; i++)
        {
            switch (i % totalInvalidUseCases)
            {
                case 0:
                    invalidInputList.Add([
                        fixture.GetInvalidInputNullName(), 
                        "Name should not be null or empty."]);
                    break;
                case 1:
                    invalidInputList.Add([
                        fixture.GetInvalidInputShortName(), 
                        "Name should not have less than 3 characters."]);
                    break;
                case 2:
                    invalidInputList.Add([
                        fixture.GetInvalidInputLongName(), 
                        "Name should not have more than 255 characters."]);
                    break;
                case 3:
                    invalidInputList.Add([
                        fixture.GetInvalidInputNullDescription(), 
                        "Description should not be null."]);
                    break;
                case 4:
                    invalidInputList.Add([
                        fixture.GetInvalidInputLongDescription(), 
                        "Description should not have more than 10000 characters."]);
                    break;
            }    
        }
        
        return invalidInputList;
    }
}