using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class UpdateUserProfileRequestDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Phone]
    public string Phone { get; set; }
    public string Address { get; set; }
}