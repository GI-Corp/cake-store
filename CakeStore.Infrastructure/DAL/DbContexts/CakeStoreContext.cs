using CakeStore.Domain.Entities.CakeStore;
using Microsoft.EntityFrameworkCore;

namespace CakeStore.Infrastructure.DAL.DbContexts;

public class CakeStoreContext : DbContext
{
    private const string CakeStoreScheme = "CakeStore";

    public CakeStoreContext(DbContextOptions<CakeStoreContext> contextOptions) : base(contextOptions)
    {
    }
    
    public DbSet<Error> Errors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}