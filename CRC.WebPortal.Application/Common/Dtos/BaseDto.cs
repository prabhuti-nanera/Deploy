namespace CRC.WebPortal.Application.Common.Dtos;

public abstract class BaseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public abstract class BaseAuditableDto : BaseDto
{
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
}
