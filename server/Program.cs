using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Helpers;
using server.Middleware;
using server.Models;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("Secret:JWTConfig"));
builder.Services.Configure<Secret>(builder.Configuration.GetSection("Secret"));


builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// 建立資料庫連線
builder.Services.AddDbContext<StoresDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 註冊服務
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IJWTHelper, JWTHelper>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ResponseService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();

        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // 允許攜帶 cookie
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 取得 JWTConfig 設定
        var jwtConfig = builder.Configuration.GetSection("Secret:JWTConfig").Get<JWTConfig>();

        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role,
            ValidateIssuer = true,
            ValidIssuer = jwtConfig?.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig?.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SignKey)),
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("authToken", out string token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var authClaims = context.Principal.Claims;
                var expClaim = authClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                if (expClaim != null)
                {
                    long exp = long.Parse(expClaim.Value);
                    DateTime expirationDate = new DateTime(exp, DateTimeKind.Utc);

                    // 檢查是否過期
                    var remainingTime = expirationDate - DateTime.UtcNow;
                    if (remainingTime < TimeSpan.FromMinutes(10))
                    {
                        // 如果剩餘時間小於10分鐘，重新生成 Token 並發送回去
                        var jwtHelper = context.HttpContext.RequestServices.GetRequiredService<IJWTHelper>();
                        var roles = authClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                        var userId = authClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        var newToken = jwtHelper.GetJWT(new JWTCliam
                        {
                            userId = int.Parse(userId),
                            exp = DateTime.UtcNow.AddMinutes(jwtConfig.ExpireMinutes).Ticks
                        }, roles);

                        var cookieOptions = jwtHelper.GenerateCookieOptions();
                        context.Response.Cookies.Append("authToken", newToken.Token, cookieOptions);
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// 初始化資料庫
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.Initialize(services);
}

app.UseHttpsRedirection();

// 使用 CORS 策略
app.UseCors("AllowSpecificOrigins");

// 使用身份驗證
app.UseAuthentication();

// 使用自訂中介軟體
app.UseMiddleware<ClaimsMiddleware>();

// 使用授權
app.UseAuthorization();

app.MapControllers();

app.Run();

