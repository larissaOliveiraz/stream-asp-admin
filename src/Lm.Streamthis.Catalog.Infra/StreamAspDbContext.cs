using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Lm.Streamthis.Catalog.Infra;

public class StreamAspDbContext(
    DbContextOptions<StreamAspDbContext> options
) : DbContext(options)
{
    public DbSet<Category> Categories() => Set<Category>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
    }
}