using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class UnitType
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; // e.g., "1 BHK", "2 BHK", "3 BHK", "Studio"
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [Range(0, int.MaxValue)]
    public int DisplayOrder { get; set; } = 0;
    
    // Foreign key
    [Required]
    public int ProjectId { get; set; }
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    
    // Methods
    public void UpdateTimestamps(string? userId = null)
    {
        if (Id == 0)
        {
            CreatedAt = DateTime.UtcNow;
        }
        else
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    // Navigation properties
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; } = null!;
    
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}
