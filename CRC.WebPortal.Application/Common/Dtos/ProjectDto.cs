namespace CRC.WebPortal.Application.Common.Dtos;

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
    public List<BuildingDto> Buildings { get; set; } = new();
    
    // Unit Management Data
    public List<string> UnitTypes { get; set; } = new();
    public List<UnitDto> GeneratedUnits { get; set; } = new();
    public NumberingPatternDto? NumberingPattern { get; set; }
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

public class UnitDto
{
    public int Id { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Legacy properties for backward compatibility
    public int Floor => FloorNumber;
    public int Position { get; set; }
    public int BuildingId { get; set; }
}

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public string AdminMobile { get; set; } = string.Empty;
    public List<CreateBuildingRequest> Buildings { get; set; } = new();
    public List<CreateUnitTypeRequest> UnitTypes { get; set; } = new();
    public CreateNumberingPatternRequest? NumberingPattern { get; set; }
    public List<CreateUnitRequest> GeneratedUnits { get; set; } = new();
}


public class CreateBuildingRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
}

public class CreateUnitRequest
{
    public string UnitNumber { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int? UnitTypeId { get; set; }
    public int? BuildingId { get; set; }
    public int ProjectId { get; set; }
}

public class CreateNumberingPatternRequest
{
    public int StartFloorNumber { get; set; } = 1;
    public int UnitNameDigits { get; set; } = 3;
    public string ApplyTo { get; set; } = "All";
    public List<CreateNumberingPatternRowRequest>? PatternRows { get; set; } = new();
}

public class CreateNumberingPatternRowRequest
{
    public string UnitType { get; set; } = string.Empty;
    
    // Legacy properties for backward compatibility
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
    public string CustomFirstDigit { get; set; } = string.Empty;
    public string CustomSecondDigit { get; set; } = string.Empty;
    public string CustomThirdDigit { get; set; } = string.Empty;
    
    // Dynamic digits support
    public List<string> Digits { get; set; } = new();
    public List<string> CustomDigits { get; set; } = new();
    public int OriginalDigitCount { get; set; } = 0;
    
    public string Result { get; set; } = string.Empty;
}

public class UpdateProjectRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public string AdminMobile { get; set; } = string.Empty;
    public List<UpdateBuildingRequest> Buildings { get; set; } = new();
    public List<UpdateUnitTypeRequest> UnitTypes { get; set; } = new();
    public NumberingPatternDto? NumberingPattern { get; set; }
}

public class UpdateBuildingRequest
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
}


public class UnitTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUnitTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public int ProjectId { get; set; }
}

public class UpdateUnitTypeRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
}

public class DeleteUnitTypeRequest
{
    public int Id { get; set; }
}

public class CloneUnitTypeRequest
{
    public int SourceUnitTypeId { get; set; }
    public string NewName { get; set; } = string.Empty;
    public string? NewDescription { get; set; }
}

public class NumberingPatternDto
{
    public int StartFloorNumber { get; set; } = 1;
    public int UnitNameDigits { get; set; } = 3;
    public string ApplyTo { get; set; } = "All";
    public List<NumberingPatternRowDto> PatternRows { get; set; } = new();
}

public class NumberingPatternRowDto
{
    public string UnitType { get; set; } = string.Empty;
    
    // Legacy properties for backward compatibility
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
    public string CustomFirstDigit { get; set; } = string.Empty;
    public string CustomSecondDigit { get; set; } = string.Empty;
    public string CustomThirdDigit { get; set; } = string.Empty;
    
    // Dynamic digits support
    public List<string> Digits { get; set; } = new();
    public List<string> CustomDigits { get; set; } = new();
    public int OriginalDigitCount { get; set; } = 0;
    
    public string Result { get; set; } = string.Empty;
}


public class UpdateUnitRequest
{
    public int Id { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
}
