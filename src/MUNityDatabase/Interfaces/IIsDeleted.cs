namespace MUNity.Database.Interfaces;

/// <summary>
/// An interface for soft-deletion
/// </summary>
public interface IIsDeleted
{
    bool IsDeleted { get; set; }
}
