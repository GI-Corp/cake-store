using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.CakeStore.Social;

public class Review : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CakeId { get; set; }
    public Cake.Cake Cake { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}