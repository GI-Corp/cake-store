namespace Shared.Domain.Entities.Abstraction;

public abstract class ReferenceEntityBase<TKey> : IEntity<TKey>
{
    /// <summary>
    ///     Gets or sets unique identifier.
    /// </summary>
    public TKey Id { get; set; }
    
    /// <summary>
    ///     Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets display name.
    /// </summary>
    public string DisplayName { get; set; }
}