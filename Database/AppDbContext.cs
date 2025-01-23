using Microsoft.EntityFrameworkCore;

namespace TwitchBot.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserStatus> UserStatus { get; set; }
    public DbSet<UserStatistics> UserStatistics { get; set; }
    public DbSet<ModuleStatistics> ModuleStatistics { get; set; }
    public DbSet<UserModuleStatistics> UserModuleStatistics { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = app.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for UserModuleUsage table
        modelBuilder.Entity<UserModuleStatistics>()
            .HasKey(umu => new { umu.UserId, umu.ModuleId });
    }


    #region Add_Values
    public async Task AddNewUser(string userId, string viewerType, string username)
    {
        if (await UserStatus.FindAsync(userId) != null)
            return;
        UserStatus.Add(new ()
        {
            UserId = userId,
            ViewerType = viewerType,
            Username = username
        });
        await SaveChangesAsync();

        await AddNewUserStatistics(userId, username);
    }

    public async Task AddNewUserStatistics(string userId, string username)
    {
        if (await UserStatistics.FindAsync(userId) != null)
            return;
        UserStatistics.Add(new ()
        {
            UserId = userId,
            Username = username,
            MessageCount = 0,
            ViewTime = 0,
            ModuleUsed = 0
        });
        await SaveChangesAsync();
    }


    public async Task AddNewModuleStatistics(string moduleId, string moduleName, int count = 0)
    {
        if (await ModuleStatistics.FindAsync(moduleId) != null)
            return;
        ModuleStatistics.Add(new ()
        {
            ModuleId = moduleId,
            ModuleName = moduleName,
            UsedCount = count
        });
        await SaveChangesAsync();
    }

    private async Task AddNewUserModuleStatistics(string userid, string moduleId, int count = 0)
    {
        var exists = await UserModuleStatistics.AnyAsync
            (us => us.UserId == userid && us.ModuleId == moduleId);
        if (exists)
            return;
        UserModuleStatistics.Add(new()
        {
            UserId = userid,
            ModuleId = moduleId,
            UsedCount = count
        });
        await SaveChangesAsync();
    }
    
        

    #endregion Add_Values

    #region Update_Values

    public async Task<bool> IncrementUserMessages(string userId)
    {
        var userStats = await UserStatistics.FirstOrDefaultAsync
            (u => u.UserId == userId);
        if (userStats == null) return false;
        
        userStats.MessageCount++;
        await SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncreaseModuleUsed(string moduleId)
    {
        var moduleStats = await ModuleStatistics.FirstOrDefaultAsync
            (m => m.ModuleId == moduleId);
        if (moduleStats == null)
            return false;
        moduleStats.UsedCount++;
        await SaveChangesAsync();
        return true;
    }

    public async Task IncreaseUserModuleUsed(string userId, string moduleId)
    {
        var userModuleStat = await UserModuleStatistics
            .FirstOrDefaultAsync(um => um.UserId == userId && um.ModuleId == moduleId);
        if (userModuleStat == null)
        {
            await AddNewUserModuleStatistics(userId, moduleId, 1);
            return;
        }

        userModuleStat.UsedCount++;
        await SaveChangesAsync();
    }
    

    #endregion Update_Values
    
    #region FetchValues

    public async Task<ViewerType> GetViewerType(string userId, string username)
    {
        var userStatus = await UserStatus.FirstOrDefaultAsync(u => u.UserId == userId);
        if (userStatus != null)
            return (ViewerType)Enum.Parse(typeof(ViewerType), userStatus.ViewerType);
        await AddNewUser(userId, ViewerType.Normal.ToString(), username);
        return ViewerType.Normal;

    }
    
    #endregion FetchValues
    
    
}