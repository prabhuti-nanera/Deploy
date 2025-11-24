using System.ComponentModel.DataAnnotations;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class Project
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // "Res", "Com", "Other"
    
    [Required]
    [MaxLength(100)]
    public string AdminName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string AdminMobile { get; set; } = string.Empty;
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public byte[]? RowVersion { get; set; }
    
    // Navigation properties
    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
    public virtual ICollection<UnitType> UnitTypes { get; set; } = new List<UnitType>();
    public virtual ICollection<NumberingPattern> NumberingPatterns { get; set; } = new List<NumberingPattern>();
    
    // Computed properties
    public int TotalBuildings => Buildings?.Count ?? 0;
    public int TotalUnits => Buildings?.Sum(b => b.TotalUnits) ?? 0;
}
