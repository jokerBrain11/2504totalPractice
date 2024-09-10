using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class VerifyEmailRequestDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }
}