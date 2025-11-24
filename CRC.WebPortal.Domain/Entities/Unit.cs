using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class Unit
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string UnitNumber { get; set; } = string.Empty; // e.g., "101", "102", "1001"
    
    [Required]
    [Range(1, 100)]
    public int Floor { get; set; }
    
    [Required]
    [Range(1, 50)]
    public int Position { get; set; } // Position on the floor (1, 2, 3, 4)
    
    // Foreign keys
    [Required]
    public int BuildingId { get; set; }
    
    public int? UnitTypeId { get; set; }
    
    // Ownership status
    [MaxLength(20)]
    public string OwnershipStatus { get; set; } = "Available"; // "Available", "Sold", "Booked", "Registered"
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
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
    
    // Navigation properties
    [ForeignKey(nameof(BuildingId))]
    public virtual Building Building { get; set; } = null!;
    
    [ForeignKey(nameof(UnitTypeId))]
    public virtual UnitType? UnitType { get; set; }
    
    public virtual UnitOwnership? Ownership { get; set; }
}
