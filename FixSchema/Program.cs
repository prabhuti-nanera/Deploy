using System;
using System.IO;
using Npgsql;

class Program
{
    static void Main()
    {
        var connectionString = "Host=localhost;Database=CRCWebPortal;Username=postgres;Password=postgres";
        var sqlScript = File.ReadAllText("../cleanup_database.sql");
        
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connected to PostgreSQL database.");
            
            using var command = new NpgsqlCommand(sqlScript, connection);
            command.ExecuteNonQuery();
            
            Console.WriteLine("✅ Database schema fixed successfully!");
            Console.WriteLine("All missing tables and columns have been added.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine($"Details: {ex}");
        }
    }
}
