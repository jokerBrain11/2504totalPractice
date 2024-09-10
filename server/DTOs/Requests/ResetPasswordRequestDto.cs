using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class ResetPasswordRequestDto
{
    [Required]
    public string NewPassword { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
}