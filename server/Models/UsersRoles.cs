namespace server.Models
{
    public class UsersRoles
    {
        /// <summary>
        /// 使用者角色編號
        /// </summary>
        public int UserRoleId { get; set; }
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 角色編號
        /// </summary>
        public int RoleId { get; set; }
    }
}