using Microsoft.EntityFrameworkCore;

namespace TwitchBot.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserStatus> UserStatus { get; set; }
    public DbSet<UserStatistics> UserStatistics { get; set; }
    public DbSet<ModuleStatistics> ModuleStatistics { get; set; }
    public DbSet<UserModuleStatistics> UserModuleStatistics { get; set; }
    
    public static AppDbContext Instance { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = app.db");
        Instance ??= this;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for UserModuleUsage table
        modelBuilder.Entity<UserModuleStatistics>()
            .HasKey(umu => new { umu.UserId, umu.ModuleId });
    }


    #region Add_Values


    public void AddNewUser(string userId, string viewerType)
    {
        UserStatus.Add(new ()
        {
            UserId = userId,
            ViewerType = viewerType
        });
        SaveChanges();
    }

    public void AddNewUserStatistics(string userId, string username)
    {
        UserStatistics.Add(new ()
        {
            UserId = userId,
            Username = username,
            MessageCount = 0,
            ViewTime = 0,
            ModuleUsed = 0
        });
        SaveChanges();
    }


    public void AddNewModuleStatistics(string moduleId, string moduleName, int count = 0)
    {
        ModuleStatistics.Add(new ()
        {
            ModuleId = moduleId,
            ModuleName = moduleName,
            UsedCount = count
        });
        SaveChanges();
    }

    public void AddNewUserModuleStatistics(string userid, string moduleId, int count = 0)
    {
        UserModuleStatistics.Add(new()
        {
            UserId = userid,
            ModuleId = moduleId,
            UsedCount = count
        });
    }
    
        

    #endregion Add_Values

    #region Update_Values

    public bool IncrementUserMessages(string userId)
    {
        var userStats = UserStatistics.FirstOrDefaultAsync
            (u => u.UserId == userId).Result;
        if (userStats == null) return false;
        
        userStats.MessageCount++;
        SaveChanges();
        return true;
    }

    public bool IncreaseModuleUsed(string moduleId)
    {
        var moduleStats = ModuleStatistics.FirstOrDefaultAsync
            (m => m.ModuleId == moduleId).Result;
        if (moduleStats == null)
            return false;
        moduleStats.UsedCount++;
        SaveChanges();
        return true;
    }

    public void IncreaseUserModuleUsed(string userId, string moduleId)
    {
        var userModuleStat = UserModuleStatistics
            .FirstOrDefaultAsync(um => um.UserId == userId && um.ModuleId == moduleId).Result;
        if (userModuleStat == null)
        {
            AddNewUserModuleStatistics(userId, moduleId, 1);
            return;
        }

        userModuleStat.UsedCount++;
        SaveChanges();
    }
    

    #endregion Update_Values
    
    
    
}