namespace CRC.WebPortal.Application.Common.Dtos;

public class UnitNumberPatternDto
{
    public int Id { get; set; }
    public string UnitType { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public int UnitNumber { get; set; }
}

public class UnitNumberPatternConfigDto
{
    public string Building { get; set; } = "All";
    public int UnitNameDigits { get; set; } = 3;
    public int StartFloorNumber { get; set; } = 1;
    public List<UnitNumberPatternDto> PatternRows { get; set; } = new();
}

public class CreateUnitNumberPatternRequest
{
    public string UnitType { get; set; } = string.Empty;
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
}
