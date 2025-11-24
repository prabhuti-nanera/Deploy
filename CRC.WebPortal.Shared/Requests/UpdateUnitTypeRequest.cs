namespace CRC.WebPortal.Shared.Requests;

public class UpdateUnitTypeRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
