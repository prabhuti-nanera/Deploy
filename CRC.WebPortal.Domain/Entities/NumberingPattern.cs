using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRC.WebPortal.Domain.Common;

namespace CRC.WebPortal.Domain.Entities;

public class NumberingPattern
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int StartFloorNumber { get; set; } = 1;
    
    [Required]
    [Range(1, 10)]
    public int UnitNameDigits { get; set; } = 3;
    
    [Required]
    [MaxLength(50)]
    public string ApplyTo { get; set; } = "All"; // "All" or specific building name
    
    // Foreign key
    [Required]
    public int ProjectId { get; set; }
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; } = null!;
    
    public virtual ICollection<NumberingPatternRow> PatternRows { get; set; } = new List<NumberingPatternRow>();
}

public class NumberingPatternRow
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UnitType { get; set; } = string.Empty; // e.g., "1BHK", "2BHK"
    
    // Legacy properties for backward compatibility
    [MaxLength(50)]
    public string FirstDigit { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string SecondDigit { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string ThirdDigit { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string CustomFirstDigit { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string CustomSecondDigit { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string CustomThirdDigit { get; set; } = string.Empty;
    
    // Dynamic digits support - stored as JSON
    [Column(TypeName = "jsonb")]
    public string DigitsJson { get; set; } = string.Empty; // JSON array of digit values
    
    [Column(TypeName = "jsonb")]
    public string CustomDigitsJson { get; set; } = string.Empty; // JSON array of custom values
    
    public int OriginalDigitCount { get; set; } = 0; // Store the digit count when row was created
    
    [MaxLength(50)]
    public string Result { get; set; } = string.Empty; // Preview result
    
    // Foreign key
    [Required]
    public int NumberingPatternId { get; set; }
    
    // Basic audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    [ForeignKey(nameof(NumberingPatternId))]
    public virtual NumberingPattern NumberingPattern { get; set; } = null!;
}
