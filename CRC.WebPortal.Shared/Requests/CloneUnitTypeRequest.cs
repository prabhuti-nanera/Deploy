namespace CRC.WebPortal.Shared.Requests;

public class CloneUnitTypeRequest
{
    public int SourceId { get; set; }
    public string NewName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}
