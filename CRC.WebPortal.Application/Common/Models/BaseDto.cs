using System;

namespace CRC.WebPortal.Application.Common.Models;

public abstract class BaseDto
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}

public abstract class BaseAuditableDto : BaseDto
{
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
