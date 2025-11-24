using CRC.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CRC.Common.Models;

public class SendOtpRequest
{
    [RequiredEmail(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;
}
