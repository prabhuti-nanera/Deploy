using System.ComponentModel.DataAnnotations;

namespace CRC.WebPortal.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    // Common audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    
    // For optimistic concurrency
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    
    // Methods
    public void UpdateTimestamps(string? userId = null)
    {
        if (Id == 0)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }
        else
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = userId;
        }
    }
}
