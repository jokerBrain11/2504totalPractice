using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace server.Helpers
{
    // PasswordHelper 用於生成安全的密碼哈希值並驗證密碼。
    public class PasswordHelper : IPasswordHelper
    {
        private readonly int bits; // 用於生成 salt 的位元數
        private readonly byte[] pepper; // 固定的 pepper 值，用來增強安全性

        // 構造函數：從配置檔案中讀取 bits 和 pepper 的值
        public PasswordHelper(IOptions<Secret> _secret)
        {
            var secret = _secret.Value;
            bits = secret.Bits;
            pepper = Convert.FromBase64String(secret.Pepper);
        }

        // Hash 方法：生成基於 salt 和 pepper 的密碼哈希
        private string Hash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000, // 設定迭代次數，增加計算強度
                numBytesRequested: 1024 / 8 // 生成 128 位元 (16 bytes) 的哈希值
            ));
        }

        // ConcatSaltAndPepper 方法：將 salt 和 pepper 組合成新的 salt
        private byte[] ConcatSaltAndPepper(byte[] salt)
        {
            byte[] newSalt = new byte[salt.Length + pepper.Length];
            Buffer.BlockCopy(salt, 0, newSalt, 0, salt.Length);
            Buffer.BlockCopy(pepper, 0, newSalt, salt.Length, pepper.Length);
            return newSalt;
        }

        // HashPassword 方法：生成加密的密碼哈希，並返回 salt 和 hash 的組合
        public string HashPassword(string password)
        {
            byte[] salt = new byte[bits / 8]; // 根據設定的 bits 生成 salt
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt); // 生成隨機的 salt
            }
            byte[] newSalt = ConcatSaltAndPepper(salt); // 將 salt 和 pepper 組合
            return $"{Convert.ToBase64String(salt)}:{Hash(password, newSalt)}"; // 返回 salt 和哈希值
        }

        // VerifyPassword 方法：驗證給定的密碼是否與存儲的哈希值匹配
        public bool VerifyPassword(string password, string hashedPassword)
        {
            string[] passwordInfo = hashedPassword.Split(':');
            if (passwordInfo.Length != 2)
            {
                return false; // 如果格式不正確，則驗證失敗
            }
            byte[] salt = Convert.FromBase64String(passwordInfo[0]); // 提取 salt
            byte[] newSalt = ConcatSaltAndPepper(salt); // 使用同樣的 salt 和 pepper 組合
            return Hash(password, newSalt) == passwordInfo[1]; // 比較生成的哈希值與存儲的哈希值
        }
    }
}
