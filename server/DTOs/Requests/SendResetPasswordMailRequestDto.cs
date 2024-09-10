using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class SendResetPasswordMailRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}