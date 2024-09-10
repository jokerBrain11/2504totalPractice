using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class ResendEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}