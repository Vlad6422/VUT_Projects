using Time2Plan.Common.Tests.Seeds;
using Time2Plan.DAL;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Common.Tests;

public class ApplicationTestingDbContext : ApplicationContext
{
    private readonly bool _seedTestingData;

    public ApplicationTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData:false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            IngredientSeeds.Seed(modelBuilder);
            RecipeSeeds.Seed(modelBuilder);
            IngredientAmountSeeds.Seed(modelBuilder);
        }
    }
}