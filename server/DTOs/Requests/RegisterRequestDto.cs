using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}