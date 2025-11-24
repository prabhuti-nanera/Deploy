using System.Text.Json;

namespace CRC.WebPortal.BlazorWebApp.Models;

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
    public int Floor { get; set; }
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
    public List<string> UnitTypes { get; set; } = new();
    public List<CreateBuildingRequest> Buildings { get; set; } = new();
    public UnitNumberingPattern? NumberingPattern { get; set; }
}


public class CreateBuildingRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
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
}

public class UpdateBuildingRequest
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Floors { get; set; }
    public int UnitsPerFloor { get; set; }
}


// Unit Numbering Pattern Models
public class UnitNumberingPattern
{
    public int StartFloorNumber { get; set; } = 1;
    public int UnitNameDigits { get; set; } = 0;
    public string ApplyTo { get; set; } = "All"; // "All" or specific building name
    public List<UnitNameDisplay> UnitNameDisplays { get; set; } = new();
}

public class UnitNameDisplay
{
    public string UnitName { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    // Legacy properties for backward compatibility
    public string FirstDigit { get; set; } = string.Empty;
    public string SecondDigit { get; set; } = string.Empty;
    public string ThirdDigit { get; set; } = string.Empty;
    public string FourthDigit { get; set; } = string.Empty;
    public string CustomFirstDigit { get; set; } = string.Empty;
    public string CustomSecondDigit { get; set; } = string.Empty;
    public string CustomThirdDigit { get; set; } = string.Empty;
    
    // Dynamic digits support
    public List<string> Digits { get; set; } = new();
    public List<string> CustomDigits { get; set; } = new();
    public int OriginalDigitCount { get; set; } = 0; // Store the digit count when row was created
    
    public string Result { get; set; } = string.Empty;
    
    // Helper methods
    public void InitializeDigits(int digitCount)
    {
        Digits = new List<string>(new string[digitCount]);
        CustomDigits = new List<string>(new string[digitCount]);
        OriginalDigitCount = digitCount; // Store the original digit count
        
        // Initialize with default values
        for (int i = 0; i < digitCount; i++)
        {
            Digits[i] = string.Empty;
            CustomDigits[i] = string.Empty;
        }
    }
    
    public string GetDigit(int index)
    {
        if (index < Digits.Count) return Digits[index];
        return string.Empty;
    }
    
    public void SetDigit(int index, string value)
    {
        while (Digits.Count <= index)
        {
            Digits.Add(string.Empty);
            CustomDigits.Add(string.Empty);
        }
        Digits[index] = value;
    }
    
    public string GetCustomDigit(int index)
    {
        if (index < CustomDigits.Count) return CustomDigits[index];
        return string.Empty;
    }
    
    public void SetCustomDigit(int index, string value)
    {
        while (CustomDigits.Count <= index)
        {
            CustomDigits.Add(string.Empty);
        }
        CustomDigits[index] = value;
    }
    
    // JSON serialization methods for database storage
    public string ToDigitsJson()
    {
        return JsonSerializer.Serialize(Digits);
    }
    
    public string ToCustomDigitsJson()
    {
        return JsonSerializer.Serialize(CustomDigits);
    }
    
    public void FromDigitsJson(string json)
    {
        if (!string.IsNullOrEmpty(json))
        {
            Digits = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
    }
    
    public void FromCustomDigitsJson(string json)
    {
        if (!string.IsNullOrEmpty(json))
        {
            CustomDigits = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
    }
}

public class CreateUnitTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class CreateUnitRequest
{
    public string UnitNumber { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public int? UnitTypeId { get; set; }
    public int? BuildingId { get; set; }
}
