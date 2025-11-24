namespace CRC.WebPortal.Application.Common.Dtos;

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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
