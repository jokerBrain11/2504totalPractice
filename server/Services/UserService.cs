using System.Text;
using System.Text.Encodings.Web;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using server.DTOs;
using server.Helpers;
using server.Middleware;
using server.Models;

namespace server.Services;

public class UserService : IUserService
{
    private readonly StoresDBContext _db;
    private readonly ResponseService _res;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailHelper _emailHelper;
    private readonly IJWTHelper _jWTHelper;
    private readonly IConfiguration _configuration;

    public UserService(
        StoresDBContext db,
        ResponseService res,
        IPasswordHelper passwordHelper,
        IHttpContextAccessor httpContextAccessor,
        IEmailHelper emailHelper,
        IJWTHelper jWTHelper,
        IConfiguration configuration)
    {
        _db = db;
        _res = res;
        _passwordHelper = passwordHelper;
        _httpContextAccessor = httpContextAccessor;
        _emailHelper = emailHelper;
        _jWTHelper = jWTHelper;
        _configuration = configuration;
    }

    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        bool havUser = _db.Users.Any(x => x.Email == request.Email);
        if (havUser)
        {
            return _res.ResponseError("信箱已存在");
        }
        bool havUsername = _db.Users.Any(x => x.Username == request.Username);
        if (havUsername)
        {
            return _res.ResponseError("使用者名稱已存在");
        }


        // 生成電子郵件確認令牌
        var token = Guid.NewGuid().ToString();
        // 令牌過期時間
        var tokenExpire = DateTime.Now.AddHours(1);

        var newUser = new Users
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = _passwordHelper.HashPassword(request.Password),
            EmailVerificationToken = token,
            EmailVerificationTokenExpires = tokenExpire,
        };


        _db.Users.Add(newUser);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        // 生成電子郵件確認令牌
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string? requestScheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        string? requestHost = _httpContextAccessor.HttpContext?.Request.Host.Value;
        string? callbackUrl = $"{requestScheme}://{requestHost}/api/user/verify-email?userId={newUser.UserId}&token={token}";

        try
        {
            // 發送包含確認鏈接的電子郵件
            await _emailHelper.SendEmailAsync(request.Email, "確認您的電子郵件",
                $"請通過 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>點擊這裡</a> 來確認您的帳戶。");
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }
        return _res.ResponseSuccess("註冊成功，請檢查您的電子郵件以驗證您的帳戶");
    }

    public async Task<IActionResult> VerifyEmail(VerifyEmailRequestDto request)
    {
        var userId = Int32.TryParse(request.UserId, out int result) ? result : 0;
        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            return _res.ResponseError("查無使用者");
        }

        if (user.EmailVerificationToken != token)
        {
            return _res.ResponseError("無效的驗證令牌");
        }

        if (user.EmailVerificationTokenExpires < DateTime.Now)
        {
            return _res.ResponseError("驗證令牌已過期");
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpires = null;
        user.UpdatedAt = DateTime.Now;

        var newUserRole = new UsersRoles
        {
            UserId = user.UserId,
            RoleId = 1
        };

        _db.UsersRoles.Add(newUserRole);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        return _res.ResponseSuccess("信箱驗證成功");
    }

    public async Task<IActionResult> ResendEmail(ResendEmailRequestDto request)
    {
        var email = request.Email;
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            return _res.ResponseError("使用者不存在");
        }

        if (user.IsEmailVerified)
        {
            return _res.ResponseError("信箱已驗證");
        }

        // 生成電子郵件確認令牌
        var token = Guid.NewGuid().ToString();
        // 令牌過期時間
        var tokenExpire = DateTime.Now.AddHours(1);

        user.EmailVerificationToken = token;
        user.EmailVerificationTokenExpires = tokenExpire;
        user.UpdatedAt = DateTime.Now;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        // 生成電子郵件確認令牌
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string? requestScheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        string? requestHost = _httpContextAccessor.HttpContext?.Request.Host.Value;
        string? callbackUrl = $"{requestScheme}://{requestHost}/api/user/verify-email?userId={user.UserId}&token={token}";

        try
        {
            // 發送包含確認鏈接的電子郵件
            await _emailHelper.SendEmailAsync(user.Email, "確認您的電子郵件",
                $"請通過 <a href='{HtmlEncoder.Default.Encode(callbackUrl)} target='_blank'>點擊這裡</a> 來確認您的帳戶。");
            return _res.ResponseSuccess("請檢查您的電子郵件以驗證您的帳戶");
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }
    }

    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Username || x.Username == request.Username);
        if (user == null)
        {
            return _res.ResponseError("查無使用者");
        }

        if (!_passwordHelper.VerifyPassword(request.Password, user.PasswordHash))
        {
            return _res.ResponseError("密碼錯誤");
        }

        if (!user.IsEmailVerified)
        {
            return _res.ResponseError("信箱未驗證");
        }

        var expireMinutes = _configuration.GetSection("Secret:JWTConfig:ExpireMinutes").Get<int>();
        var jwtCliam = new JWTCliam
        {
            exp = DateTime.Now.AddMinutes(expireMinutes).Ticks,
            userId = user.UserId,
            email = user.Email
        };

        var roles = _db.Roles.Join(
            _db.UsersRoles,
            r => r.RoleId,
            ur => ur.RoleId,
            (r, ur) => new { r, ur }).Where(x => x.ur.UserId == user.UserId).Select(x => x.r.RoleName).ToList();
        var jwt = _jWTHelper.GetJWT(jwtCliam, roles);
        var cookieOptions = _jWTHelper.GenerateCookieOptions();

        if (!jwt.Success)
        {
            return _res.ResponseError(jwt.Message);
        }
        _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", jwt.Token, cookieOptions);

        var res = new LoginResponseDto
        {
            Username = user.Username,
            Roles = roles
        };

        return _res.ResponseMessage(ResponseStatus.Success, res);
    }

    public async Task<IActionResult> Logout()
    {
        var cookieOptions = _jWTHelper.GenerateCookieOptions();
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("authToken", cookieOptions);
        return _res.ResponseSuccess("登出成功");
    }

    public async Task<IActionResult> GetProfile(UserInfoModel userInfo)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userInfo.UserId);
        if (user == null)
        {
            return _res.ResponseError("使用者不存在");
        }
        var userDto = new GetProfileResponseDto
        {
            Email = user.Email,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Address = user.Address
            // Role = user.Role
        };

        var res = _res.ResponseMessage(ResponseStatus.Success, userDto);
        return res;
    }

    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequestDto request, UserInfoModel userInfo)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userInfo.UserId);
        if (user == null)
        {
            return _res.ResponseError("使用者不存在");
        }

        if (request.Email != user.Email)
        {
            var havUser = _db.Users.Any(x => x.Email == request.Email);
            if (havUser)
            {
                return _res.ResponseError("信箱已存在");
            }
        }
        if (request.Username != user.Username)
        {
            var havUsername = _db.Users.Any(x => x.Username == request.Username);
            if (havUsername)
            {
                return _res.ResponseError("使用者名稱已存在");
            }
        }

        if (!string.IsNullOrEmpty(user.Phone) && request.Phone != user.Phone)
        {
            var havPhone = _db.Users.Any(x => x.Phone == request.Phone);
            if (havPhone)
            {
                return _res.ResponseError("電話號碼已存在");
            }
        }

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;
        user.Address = request.Address;
        user.UpdatedAt = DateTime.Now;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        return _res.ResponseSuccess("更新成功");
    }

    public async Task<IActionResult> SendResetPasswordMail(SendResetPasswordMailRequestDto request)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == request.Email);
        if (user == null)
        {
            _res.ResponseError("查無此信箱使用者");
        }

        if (user.IsEmailVerified == false)
        {
            return _res.ResponseError("信箱未驗證，無法重設密碼");
        }

        var token = Guid.NewGuid().ToString();
        var tokenExpire = DateTime.Now.AddHours(1);

        user.PasswordResetToken = token;
        user.PasswordResetTokenExpires = tokenExpire;
        user.UpdatedAt = DateTime.Now;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string clientDomain = _configuration.GetSection("CORS:Origins").Get<string[]>().FirstOrDefault();
        string callbackUrl = $"{clientDomain}/reset?userId={user.UserId}&token={token}";

        try
        {
            await _emailHelper.SendEmailAsync(request.Email, "重設您的密碼",
                $"請通過 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' target='_blank'>點擊這裡</a> 來重設您的密碼。");
            return _res.ResponseSuccess("請檢查您的電子郵件以重設您的密碼");
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }
    }

    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
    {
        var userId = Int32.TryParse(request.UserId, out int result) ? result : 0;
        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            return _res.ResponseError("查無使用者");
        }

        if (user.PasswordResetToken != token)
        {
            return _res.ResponseError("無效的重設密碼令牌");
        }

        if (user.PasswordResetTokenExpires < DateTime.Now)
        {
            return _res.ResponseError("重設密碼令牌已過期");
        }

        user.PasswordHash = _passwordHelper.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;
        user.UpdatedAt = DateTime.Now;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return _res.ResponseError(e.Message);
        }

        return _res.ResponseSuccess("重設密碼成功");
    }
}