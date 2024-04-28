using CakeStore.Domain.Entities.CakeStore.Media;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeStore.Infrastructure.EntityConfigurations;

public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> imageConfiguration)
    {
        imageConfiguration.ToTable(nameof(CakeStoreContext.Images), CakeStoreContext.CakeStoreScheme);
        imageConfiguration.HasKey(i => i.Id);
        imageConfiguration.Property(i => i.Id).ValueGeneratedOnAdd();
        imageConfiguration.Property(i => i.Name).IsRequired().HasMaxLength(100);
        imageConfiguration.Property(i => i.ImageUrl).IsRequired().HasMaxLength(255);
        imageConfiguration.Property(i => i.IsActive).IsRequired();

        imageConfiguration.HasOne(i => i.Cake)
            .WithMany(c => c.CakeImages)
            .HasForeignKey(i => i.CakeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
