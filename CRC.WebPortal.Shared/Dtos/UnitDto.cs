namespace CRC.WebPortal.Shared.Dtos;

public class UnitDto
{
    public int Id { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string? UnitType { get; set; }
    public int FloorNumber { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int Position { get; set; }
    public int? BuildingId { get; set; }
    public string OwnershipStatus { get; set; } = "Available";
    public UnitOwnershipDto? Ownership { get; set; }
    public DateTime CreatedAt { get; set; }
}
