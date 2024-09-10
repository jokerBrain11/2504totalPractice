namespace server.DTOs;
public class LoginResponseDto
{
    public string Username { get; set; }
    public List<string> Roles { get; set; }
}