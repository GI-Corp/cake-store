using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.CakeStore.Media;

public class Image : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public Guid? CakeId { get; set; }
    public Cake.Cake Cake { get; set; }
    public bool IsActive { get; set; }
    public int Ordering { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Cake.Cake> Cakes { get; set; } = new();
}