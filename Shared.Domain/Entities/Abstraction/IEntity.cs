namespace Shared.Domain.Entities.Abstraction;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}