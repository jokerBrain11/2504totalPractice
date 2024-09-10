namespace server.Helpers
{
    public class JWTConfig
    {
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public string SignKey { set; get; }
        public int ExpireMinutes { set; get; }
    }

    public class JWTCliam
    {
        /// <summary>
        /// 聲明資訊-發行者
        /// </summary>
        /// <value></value>
        public string? iss { set; get; } = "nutc2504";

        /// <summary>
        /// 聲明資訊-User內容
        /// </summary>
        /// <value></value>
        public string? sub { set; get; }

        /// <summary>
        /// 聲明資訊-接收者
        /// </summary>
        /// <value></value>
        public string? aud { set; get; }

        /// <summary>
        /// 聲明資訊-有效期限
        /// </summary>
        /// <value></value>
        public long exp { set; get; }

        /// <summary>
        /// 聲明資訊-起始時間
        /// </summary>
        /// <value></value>
        public string? nbf { set; get; }

        /// <summary>
        /// 聲明資訊-發行時間
        /// </summary>
        /// <value></value>
        public string? iat { set; get; } = "720";

        /// <summary>
        /// 聲明資訊-獨立識別ID
        /// </summary>
        /// <value></value>
        public string? jti { set; get; }

        /// <summary>
        /// 聲明資訊-使用者代號
        /// </summary>
        /// <value></value>
        public int userId { set; get; }

        /// <summary>
        /// 聲明資訊-電子郵件
        /// </summary>
        /// <value></value>
        public string? email { set; get; }
    }

    public class RunStatus
    {
        /// <summary>
        /// Token是否有成功產生狀態
        /// </summary>
        /// <value>布林值</value>
        public bool Success { set; get; }

        /// <summary>
        /// Token
        /// </summary>
        /// <value>字串</value>
        public string Token { set; get; }

        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <value>字串</value>
        public string? Message { set; get; }
    }
}