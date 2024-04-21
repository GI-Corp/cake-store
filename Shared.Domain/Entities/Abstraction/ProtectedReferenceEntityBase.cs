namespace Shared.Domain.Entities.Abstraction;

public abstract class ProtectedReferenceEntityBase<TKey>
{
    /// <summary>
    ///  Gets or sets unique identifier.
    /// </summary>
    public TKey Id { get; private set; }

    /// <summary>
    ///  Gets or sets name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///  Gets or sets display name.
    /// </summary>
    public string DisplayName { get; private set; }
}