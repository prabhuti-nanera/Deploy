using System.ComponentModel.DataAnnotations;

namespace CRC.WebPortal.Shared.Dtos;

public class UnitOwnershipDto
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerMobile { get; set; } = string.Empty;
    public string? SellingDetails { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? SaleDate { get; set; }
    public string SaleStatus { get; set; } = "Sold";
    public int UnitId { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string BuildingName { get; set; } = string.Empty;
    public string? UnitType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateUnitOwnershipDto
{
    [Required(ErrorMessage = "Owner name is required")]
    [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
    public string OwnerName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string OwnerEmail { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mobile number is required")]
    [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
    public string OwnerMobile { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Selling details cannot exceed 500 characters")]
    public string? SellingDetails { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Sale price must be a positive number")]
    public decimal? SalePrice { get; set; }
    
    public DateTime? SaleDate { get; set; }
    
    [Required(ErrorMessage = "Sale status is required")]
    public string SaleStatus { get; set; } = "Sold";
    
    [Required(ErrorMessage = "Unit ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid unit")]
    public int UnitId { get; set; }
}

public class UpdateUnitOwnershipDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Owner name is required")]
    [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
    public string OwnerName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string OwnerEmail { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mobile number is required")]
    [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
    public string OwnerMobile { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Selling details cannot exceed 500 characters")]
    public string? SellingDetails { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Sale price must be a positive number")]
    public decimal? SalePrice { get; set; }
    
    public DateTime? SaleDate { get; set; }
    
    [Required(ErrorMessage = "Sale status is required")]
    public string SaleStatus { get; set; } = "Sold";
}
