using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using server.DTOs;
using server.Middleware;
using server.Services;

namespace server.Controllers;

[Route("api/[controller]")] // 設置路由
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 註冊 API 端點
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.Register(request);
        return res;
    }

    /// <summary>
    /// 信箱驗證 API 端點
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] VerifyEmailRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.VerifyEmail(request);
        return res;
    }

    /// <summary>
    /// 重新發送驗證信 API 端點
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("resend-email")]
    public async Task<IActionResult> ResendEmail([FromBody] ResendEmailRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.ResendEmail(request);
        return res;
    }

    /// <summary>
    /// 登入 API 端點
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.Login(request);
        return res;
    }

    /// <summary>
    /// 檢查登入 API 端點
    /// </summary>
    /// <returns></returns>
    [HttpGet("check-login")]
    public IActionResult CheckLogin()
    {
        var userInfo = HttpContext.Items["UserInfo"] as UserInfoModel;
        if (userInfo == null)
        {
            return Unauthorized(new { isAuthenticated = false });
        }
        return Ok(new { isAuthenticated = true });
    }

    /// <summary>
    /// 登出 API 端點
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        var res = await _userService.Logout();
        return res;
    }

    /// <summary>
    /// 取得使用者資訊 API 端點
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("user-profile")]
    public async Task<IActionResult> UserProfile()
    {
        var userInfo = HttpContext.Items["UserInfo"] as UserInfoModel;
        if (userInfo == null)
        {
            return BadRequest("無法取得使用者資訊");
        }
        var res = await _userService.GetProfile(userInfo);
        return res;
    }

    /// <summary>
    /// 取得使用者資訊 API 端點
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("update-userProfile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequestDto request)
    {
        var userInfo = HttpContext.Items["UserInfo"] as UserInfoModel;
        if (userInfo == null)
        {
            return BadRequest("無法取得使用者資訊");
        }
        var res = await _userService.UpdateUserProfile(request, userInfo);
        return res;
    }

    /// <summary>
    /// 重發重設密碼信 API 端點
    /// </summary>
    [HttpPost("send-Reset-password-mail")]
    public async Task<IActionResult> SendResetPasswordMail([FromBody] SendResetPasswordMailRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.SendResetPasswordMail(request);
        return res;
    }

    /// <summary>
    /// 重設密碼 API 端點
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var res = await _userService.ResetPassword(request);
        return res;
    }
}
