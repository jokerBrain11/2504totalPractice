using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server.Models;
using System.Threading.Tasks;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        // 從服務提供者中獲取 DbContext
        using var context = serviceProvider.GetRequiredService<StoresDBContext>();

        // 確保資料庫已創建
        await context.Database.EnsureCreatedAsync();

        // 檢查是否已經存在角色
        if (!await context.Roles.AnyAsync(x => x.RoleName == "User"))
        {
            var userRole = new Roles { RoleName = "User" };
            await context.Roles.AddAsync(userRole);
        }

        if (!await context.Roles.AnyAsync(x => x.RoleName == "Admin"))
        {
            var adminRole = new Roles { RoleName = "Admin" };
            await context.Roles.AddAsync(adminRole);
        }

        // 儲存變更
        await context.SaveChangesAsync();


        // 檢查是否已經存在admin帳號
        // int adminRoleId = await context.Roles.Where(x => x.RoleName == "Admin").Select(x => x.RoleId).FirstOrDefaultAsync();
        // var adminUserIds = await context.UsersRoles.Where(x => x.RoleId == adminRoleId).Select(x => x.UserId).ToListAsync();
    }
}
