using Bogus;
using Lm.Streamthis.Catalog.Infra;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.IntegrationTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; } = new("pt_BR");

    public StreamAspDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new StreamAspDbContext(
            new DbContextOptionsBuilder<StreamAspDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );

        if (preserveData == false)
            dbContext.Database.EnsureDeleted();

        return dbContext;
    }
}
