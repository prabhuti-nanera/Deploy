using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class UnitOwnership
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string OwnerName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string OwnerEmail { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string OwnerMobile { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? SellingDetails { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? SalePrice { get; set; }
    
    public DateTime? SaleDate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string SaleStatus { get; set; } = "Sold"; // "Sold", "Booked", "Registered"
    
    // Foreign key
    [Required]
    public int UnitId { get; set; }
    
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
    [ForeignKey(nameof(UnitId))]
    public virtual Unit Unit { get; set; } = null!;
}
