using System.Data.SQLite;

namespace TwitchBot.Database;

public class Database
{
    public static Database Instance { get; private set; }
    
    public Database()
    {
        Instance ??= this;
    }

    public async Task InitializeUsers()
    {
        // Specify the database file
        const string connectionString = "Data Source=users.db;Version=3;";
        await using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            
            // Create the UserStatus table if it doesn't exist
            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS UserStatus (
                UserId TEXT PRIMARY KEY,
                ViewerType TEXT
            );";
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public async Task AddOrUpdateUser(string userId, int viewerType)
    {
        const string connectionString = "Data Source=users.db;Version=3;";
        await using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Insert or update the user's status
            string query = @"
            INSERT INTO UserStatus (UserId, ViewerType)
            VALUES (@UserId, @ViewerType)
            ON CONFLICT(UserId) DO UPDATE SET ViewerType = excluded.ViewerType;
        ";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ViewerType", viewerType);
                command.ExecuteNonQuery();
            }
        }
    }

    public async Task<ViewerType> GetViewerType(string userId)
    {
        const string connectionString = "Data Source=users.db;Version=3;";
        await using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT ViewerType FROM UserStatus WHERE UserId = @UserId;";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (ViewerType) Enum.Parse(typeof(ViewerType), reader.GetString(0)); // Return ViewerType
                    }
                }
            }
        }
        return ViewerType.Normal; // Default if user is not found
    }
}