namespace server.Models;

public class Users
{
    /// <summary>
    /// 使用者編號
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// 使用者密碼
    /// </summary>
    public string PasswordHash { get; set; }
    /// <summary>
    /// 使用者電子郵件
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// 使用者姓氏
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// 使用者名字
    /// </summary>
    public string? LastName { get; set; }
    /// <summary>
    /// 使用者電話
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// 使用者地址
    /// </summary>
    public string? Address { get; set; }
    /// <summary>
    /// 是否電子郵件已驗證
    /// </summary>
    public bool IsEmailVerified { get; set; }
    /// <summary>
    /// 電子郵件驗證碼
    /// </summary>
    public string? EmailVerificationToken { get; set; }
    /// <summary>
    /// 電子郵件驗證碼過期時間
    /// </summary>
    public DateTime? EmailVerificationTokenExpires { get; set; }
    /// <summary>
    /// 重設密碼驗證碼
    /// </summary>
    public string? PasswordResetToken { get; set; }
    /// <summary>
    /// 重設密碼驗證碼過期時間
    /// </summary>
    public DateTime? PasswordResetTokenExpires { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}