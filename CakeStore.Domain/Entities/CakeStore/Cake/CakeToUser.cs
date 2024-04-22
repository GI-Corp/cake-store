using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.CakeStore.Cake;

public class CakeToUser : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CakeId { get; set; }
    public Cake Cake { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}