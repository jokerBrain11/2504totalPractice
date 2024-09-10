using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace server.Middleware;

public class ClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public ClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 取得當前使用者的 Claims
        var user = context.User;
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var claims = user.Claims;
            var userId = int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value, out int id);
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value) as List<string>;
            var userInfo = new UserInfoModel
            {
                UserId = id,
                Email = email,
                UserRole = roles
            };
            context.Items["UserInfo"] = userInfo;
        }

        await _next(context);
    }
}