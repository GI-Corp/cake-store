namespace Shared.Domain.Entities.Abstraction;

public abstract class ReferenceEntityBase<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
}