using Bogus;

namespace Lm.Streamthis.Catalog.IntegrationTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; } = new("pt_BR");
}