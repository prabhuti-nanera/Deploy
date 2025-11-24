namespace CRC.WebPortal.Shared.Requests;

public class CreateUnitRequest
{
    public string UnitNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int Position { get; set; }
    public int BuildingId { get; set; }
    public int? UnitTypeId { get; set; }
}
