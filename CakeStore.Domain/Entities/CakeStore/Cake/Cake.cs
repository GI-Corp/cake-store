using CakeStore.Domain.Entities.CakeStore.Media;
using CakeStore.Domain.Entities.CakeStore.Social;
using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.CakeStore.Cake;

public class Cake : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Image> CakeImages { get; set; } = new List<Image>();
    public List<Reaction> CakeReactions { get; set; } = new List<Reaction>();
    public List<Review> CakeReviews { get; set; } = new List<Review>();
}