using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace server.Helpers;

/// <summary>
/// 提供 JWT 相關服務的幫助類別
/// </summary>
public class JWTHelper : IJWTHelper
{
    private readonly ILogger<JWTHelper> log;
    private readonly JWTConfig _jwtConfig;

    /// <summary>
    /// 構造函數，初始化 JWTServices 類別
    /// </summary>
    /// <param name="logger">用於記錄日誌的 ILogger 實例</param>
    public JWTHelper(ILogger<JWTHelper> logger, IOptions<JWTConfig> jwtConfig)
    {
        log = logger;
        _jwtConfig = jwtConfig.Value;
    }

    /// <summary>
    /// 生成 Cookie 選項
    /// </summary>
    /// <param name="Expires">Cookie 過期時間（分鐘）</param>
    /// <returns>CookieOptions 實例</returns>
    /// <remarks>Expires 默認值為 20 分鐘</remarks>
    public CookieOptions GenerateCookieOptions(int Expires = 20)
    {
        return new CookieOptions
        {
            // Expires = DateTime.Now.AddMinutes(Expires),
            Expires = DateTime.Now.AddMinutes(Expires),
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = false
        };
    }

    /// <summary>
    /// 生成 Token 的資訊聲明集合
    /// </summary>
    /// <param name="jWTCliam">Token 的資訊聲明內容物件</param>
    /// <returns>包含資訊聲明的列表</returns>
    private List<Claim> GenCliams(JWTCliam jwtCliam, List<string> roles)
    {
        // 初始化 claims 列表
        List<Claim> claims = new List<Claim>();

        // 定義一個字典來映射 JWTCliam 屬性到對應的 Claim 類型
        var claimMappings = new Dictionary<string, string>
            {
                { nameof(jwtCliam.aud), JwtRegisteredClaimNames.Aud },
                { nameof(jwtCliam.exp), JwtRegisteredClaimNames.Exp },
                { nameof(jwtCliam.iat), JwtRegisteredClaimNames.Iat },
                { nameof(jwtCliam.iss), JwtRegisteredClaimNames.Iss },
                { nameof(jwtCliam.jti), JwtRegisteredClaimNames.Jti },
                { nameof(jwtCliam.nbf), JwtRegisteredClaimNames.Nbf },
                { nameof(jwtCliam.sub), JwtRegisteredClaimNames.Sub },
                { nameof(jwtCliam.userId), ClaimTypes.Name },
                { nameof(jwtCliam.email), ClaimTypes.Email }
            };

        // 遍歷字典並添加對應的 Claim
        foreach (var mapping in claimMappings)
        {
            // 如果 JWTCliam 的屬性值不為空，則添加到 claims 列表
            var value = jwtCliam.GetType().GetProperty(mapping.Key).GetValue(jwtCliam)?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                claims.Add(new Claim(mapping.Value, value));
            }
        }

        // 添加角色聲明
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    /// <summary>
    /// 生成 JWT Token
    /// </summary>
    /// <param name="jwtCliam">Token 的資訊聲明內容物件</param>
    /// <returns>包含生成結果和 Token 字串的 RunStatus</returns>
    public RunStatus GetJWT(JWTCliam jwtCliam, List<string> roles)
    {
        RunStatus response = new RunStatus();
        string signKey = _jwtConfig.SignKey; // 用於加密的密鑰
        string issuer = _jwtConfig.Issuer; // Token 的發行者
        string audience = _jwtConfig.Audience; // Token 的接收對象
        int expireMinutes = _jwtConfig.ExpireMinutes; // Token 的過期時間（分鐘）

        try
        {
            // 生成 Token 所需的資訊聲明
            List<Claim> claims = GenCliams(jwtCliam, roles);
            ClaimsIdentity userClaimsIdentity = new ClaimsIdentity(claims);

            // 設定加密密鑰
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
            // 使用 HmacSha256 進行加密
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 設定 Token 的描述信息
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer, // 設置發行者資訊
                Audience = audience, // 設置驗證發行者對象
                NotBefore = DateTime.Now, // 設置可用時間，預設值是當前時間
                IssuedAt = DateTime.Now, // 設置發行時間，預設值是當前時間
                Subject = userClaimsIdentity, // Token 的用戶資訊內容物件
                Expires = DateTime.Now.AddMinutes(expireMinutes), // 設置 Token 的有效期限
                SigningCredentials = signingCredentials // Token 的簽章
            };

            // 創建 JWT Token 處理器
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // 創建 Token
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            // 序列化 Token 為字串
            string serializeToken = tokenHandler.WriteToken(securityToken);

            // 設置回應結果
            response.Success = true;
            response.Token = serializeToken;
            response.Message = null;
        }
        catch (Exception ex)
        {
            // 記錄錯誤信息
            log.LogError($"{ex.Message}\n{ex.StackTrace}");
            response.Success = false;
            response.Token = string.Empty;
            response.Message = "產生 Token 過程發生錯誤.";
        }

        return response;
    }
}

