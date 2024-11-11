using Domain.Abstractions;

namespace Core.Abstractions;

public abstract class Entity : ISoftDeletable, IAuditable
{
    public int Id { get; init; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}