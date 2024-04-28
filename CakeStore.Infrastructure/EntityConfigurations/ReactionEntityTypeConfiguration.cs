using CakeStore.Domain.Entities.CakeStore.Social;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeStore.Infrastructure.EntityConfigurations;

public class ReactionEntityTypeConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> reactionConfiguration)
    {
        reactionConfiguration.ToTable(nameof(CakeStoreContext.Reactions), CakeStoreContext.CakeStoreScheme);
        reactionConfiguration.HasKey(r => r.Id);
        reactionConfiguration.Property(r => r.Id).ValueGeneratedOnAdd();
        reactionConfiguration.Property(r => r.Type).IsRequired().HasMaxLength(50);
        reactionConfiguration.HasIndex(r => new { r.UserId, r.CakeId });
        reactionConfiguration.Property(r => r.UserId).IsRequired();
        reactionConfiguration.Property(r => r.CakeId).IsRequired();

        reactionConfiguration.HasOne(r => r.Cake)
            .WithMany(c => c.CakeReactions)
            .HasForeignKey(r => r.CakeId)
            .OnDelete(DeleteBehavior.Cascade);

        reactionConfiguration.Property(r => r.IsActive).IsRequired();
    }
}
