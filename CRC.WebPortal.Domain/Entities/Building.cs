using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class Building
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // "A", "B", etc.
    
    [Required]
    [Range(1, 100)]
    public int Floors { get; set; }
    
    [Required]
    [Range(1, 50)]
    public int UnitsPerFloor { get; set; }
    
    // Foreign key
    [Required]
    public int ProjectId { get; set; }
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public byte[]? RowVersion { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; } = null!;
    
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
    
    // Computed properties
    public int TotalUnits => Floors * UnitsPerFloor;
}
