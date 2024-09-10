namespace server.Helpers
{
    /// <summary>
    /// 提供 JWT 相關服務的幫助類別
    /// </summary>
    public interface IJWTHelper
    {
        public CookieOptions GenerateCookieOptions(int Expires = 20);
        public RunStatus GetJWT(JWTCliam jwtCliam, List<string> roles);
    }
}
