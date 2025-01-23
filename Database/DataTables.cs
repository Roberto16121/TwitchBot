using System.ComponentModel.DataAnnotations;

namespace TwitchBot.Database;

public class UserStatus
{
    [Key]
    public string UserId { get; set; }
    public string Username { get; set; }
    public string ViewerType { get; set; }
}

public class UserStatistics
{
    [Key]
    public string UserId { get; set; }
    public string Username { get; set; }
    public int MessageCount { get; set; }
    public int ViewTime { get; set; } //Seconds
    public int ModuleUsed { get; set; }
}

public class ModuleStatistics
{
    [Key]
    public string ModuleId { get; set; }
    public string ModuleName { get; set; }
    public int UsedCount { get; set; }
}

public class UserModuleStatistics
{
    [Key]
    public string UserId { get; set; }
    public string ModuleId { get; set; }
    public int UsedCount { get; set; }
}