using System;
namespace Shared.Domain.Entities.Abstraction;

public interface IBaseEntity<TId>
{
    TId Id { get; set; }
    bool IsActive { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
}
