namespace server.Helpers;

public interface IPasswordHelper
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}