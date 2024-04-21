using Microsoft.EntityFrameworkCore;

namespace CakeStore.Infrastructure.DAL.DbContexts;

public class CakeStoreContext : DbContext
{
    private const string CakeStoreScheme = "CakeStore";

    public CakeStoreContext(DbContextOptions<CakeStoreContext> contextOptions) : base(contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}