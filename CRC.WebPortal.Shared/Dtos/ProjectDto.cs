namespace CRC.WebPortal.Shared.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public string AdminMobile { get; set; } = string.Empty;
    public int TotalBuildings { get; set; }
    public int TotalUnits { get; set; }
    public DateTime CreatedAt { get; set; }
    public UnitNumberingPatternDto? NumberingPattern { get; set; }
    public List<BuildingDto>? Buildings { get; set; }
    public List<UnitDto>? Units { get; set; }
    public List<UnitTypeDto>? UnitTypes { get; set; }
    public List<UnitDto>? GeneratedUnits { get; set; }
}


public class BuildingDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
    public int TotalUnits { get; set; }
    public int ProjectId { get; set; }
    public List<UnitDto> Units { get; set; } = new();
}
