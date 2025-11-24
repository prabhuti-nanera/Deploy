namespace CRC.WebPortal.Shared.Requests;

public class UpdateProjectRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public string AdminMobile { get; set; } = string.Empty;
    public List<UpdateBuildingRequest>? Buildings { get; set; }
    public List<string>? UnitTypes { get; set; }
    public UpdateNumberingPatternRequest? NumberingPattern { get; set; }
    public List<GeneratedUnitRequest>? GeneratedUnits { get; set; }
}

public class UpdateBuildingRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
}

public class UpdateNumberingPatternRequest
{
    public int StartFloorNumber { get; set; } = 1;
    public int UnitNameDigits { get; set; } = 3;
    public string ApplyTo { get; set; } = "All";
    public List<UpdateNumberingPatternRowRequest> PatternRows { get; set; } = new();
}

public class UpdateNumberingPatternRowRequest
{
    public string UnitType { get; set; } = string.Empty;
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
    public string CustomFirstDigit { get; set; } = string.Empty;
    public string CustomSecondDigit { get; set; } = string.Empty;
    public string CustomThirdDigit { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    // Support for dynamic digits
    public string DigitsJson { get; set; } = string.Empty;
    public string CustomDigitsJson { get; set; } = string.Empty;
    public int OriginalDigitCount { get; set; } = 3;
}

public class GeneratedUnitRequest
{
    public string UnitNumber { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int Position { get; set; }
}
