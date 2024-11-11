namespace Core.Abstractions;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
