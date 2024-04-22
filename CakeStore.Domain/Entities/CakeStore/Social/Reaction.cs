using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.CakeStore.Social;

public class Reaction : IBaseEntity<int>
{
    public int Id { get; set; }
    public string Type { get; set; }
    public Guid UserId { get; set; }
    public Guid CakeId { get; set; }
    public Cake.Cake Cake { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}