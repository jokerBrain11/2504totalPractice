using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Middleware;

namespace server.Services;

public interface IUserService
{
    Task<IActionResult> Register(RegisterRequestDto request);
    Task<IActionResult> VerifyEmail(VerifyEmailRequestDto request);
    Task<IActionResult> ResendEmail(ResendEmailRequestDto request);
    Task<IActionResult> Login(LoginRequestDto request);
    Task<IActionResult> Logout();
    Task<IActionResult> GetProfile(UserInfoModel userInfo);
    Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequestDto request, UserInfoModel userInfo);
    Task<IActionResult> SendResetPasswordMail(SendResetPasswordMailRequestDto request);
    Task<IActionResult> ResetPassword(ResetPasswordRequestDto request);
}