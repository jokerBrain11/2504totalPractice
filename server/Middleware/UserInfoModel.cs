namespace server.Middleware;
public class UserInfoModel
{
    /// <summary>
    /// 使用者ID
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 信箱
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// 使用者角色
    /// </summary>
    public List<string> UserRole { get; set; }
}
