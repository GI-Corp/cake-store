using CakeStore.Domain.Entities.CakeStore.Social;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeStore.Infrastructure.EntityConfigurations;

public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> reviewConfiguration)
    {
        reviewConfiguration.ToTable(nameof(CakeStoreContext.Reviews), CakeStoreContext.CakeStoreScheme);
        reviewConfiguration.HasKey(r => r.Id);
        reviewConfiguration.Property(r => r.Id).ValueGeneratedOnAdd();
        reviewConfiguration.Property(r => r.Text).HasMaxLength(2048);
        reviewConfiguration.Property(r => r.UserId).IsRequired();
        reviewConfiguration.Property(r => r.CakeId).IsRequired();

        reviewConfiguration.HasOne(r => r.Cake)
            .WithMany(c => c.CakeReviews)
            .HasForeignKey(r => r.CakeId)
            .OnDelete(DeleteBehavior.Cascade);

        reviewConfiguration.Property(r => r.IsActive).IsRequired();
    }
}
