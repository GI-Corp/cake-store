using CakeStore.Domain.Entities.CakeStore.Cake;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeStore.Infrastructure.EntityConfigurations;

public class CakeToUserEntityTypeConfiguration : IEntityTypeConfiguration<CakeToUser>
{
    public void Configure(EntityTypeBuilder<CakeToUser> cakeToUserConfiguration)
    {
        cakeToUserConfiguration.ToTable(nameof(CakeStoreContext.CakeToUsers), CakeStoreContext.CakeStoreScheme);
        cakeToUserConfiguration.HasKey(ctu => ctu.Id);
        cakeToUserConfiguration.Property(ctu => ctu.Id).ValueGeneratedOnAdd();
        cakeToUserConfiguration.Property(ctu => ctu.UserId).IsRequired();
        cakeToUserConfiguration.Property(ctu => ctu.CakeId).IsRequired();
        cakeToUserConfiguration.Property(ctu => ctu.IsActive).IsRequired();
    }
}
