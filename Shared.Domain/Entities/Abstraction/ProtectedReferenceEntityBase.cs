namespace Shared.Domain.Entities.Abstraction;

public abstract class ProtectedReferenceEntityBase<TKey>
{
    public TKey Id { get; private set; }
    public string Name { get; private set; }
    public string DisplayName { get; private set; }
}