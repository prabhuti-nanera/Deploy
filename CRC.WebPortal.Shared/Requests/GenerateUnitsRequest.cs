namespace CRC.WebPortal.Shared.Requests;

public class GenerateUnitsRequest
{
    public int ProjectId { get; set; }
    public List<UnitGenerationData> UnitsData { get; set; } = new();
}

public class UnitGenerationData
{
    public string UnitNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int Position { get; set; }
    public string? UnitType { get; set; }
}
