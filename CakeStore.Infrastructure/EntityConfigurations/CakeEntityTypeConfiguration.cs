using CakeStore.Domain.Entities.CakeStore.Cake;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeStore.Infrastructure.EntityConfigurations;

public class CakeEntityTypeConfiguration : IEntityTypeConfiguration<Cake>
{
    public void Configure(EntityTypeBuilder<Cake> cakeConfiguration)
    {
        cakeConfiguration.ToTable(nameof(CakeStoreContext.Cakes), CakeStoreContext.CakeStoreScheme);
        
        cakeConfiguration.HasKey(c => c.Id);
        cakeConfiguration.Property(c => c.Id).ValueGeneratedOnAdd();
        cakeConfiguration.Property(c => c.Name).IsRequired().HasMaxLength(100);
        cakeConfiguration.Property(c => c.Description).HasMaxLength(500);
        cakeConfiguration.Property(c => c.Price).HasColumnType("decimal(18,2)").IsRequired();
        cakeConfiguration.Property(c => c.Amount).IsRequired();
        cakeConfiguration.Property(c => c.IsActive).IsRequired();
    }
}
