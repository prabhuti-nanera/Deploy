namespace CRC.WebPortal.Shared.Requests;

public class CreateUnitTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int ProjectId { get; set; }
}
