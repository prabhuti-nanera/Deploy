namespace CRC.WebPortal.Shared.Dtos;

public class UnitNumberingPatternDto
{
    public string Pattern { get; set; } = string.Empty;
    public string Separator { get; set; } = string.Empty;
    public int UnitNameDigits { get; set; } = 3;
    public int StartFloorNumber { get; set; } = 1;
    public string ApplyTo { get; set; } = "All";
    public List<PatternRowDto> PatternRows { get; set; } = new();
}

public class PatternRowDto
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
