using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ploch.Data.EFCore.SqlServer;
using Ploch.MyApp.Data;

namespace Ploch.MyApp.Data.SqlServer;

/// <summary>
/// Factory for creating MyAppDbContext instances configured for SQL Server.
/// </summary>
public class MyAppDbContextFactory : SqlServerDbContextFactory<MyAppDbContext, MyAppDbContextFactory>
{
    /// <summary>
    /// Default constructor for design-time usage.
    /// </summary>
    public MyAppDbContextFactory() : base(options => new MyAppDbContext(options), GetConnectionString)
    {
    }

    /*
    /// <summary>
    /// Ensures SQL Server provider is configured for design-time operations.
    /// </summary>
    protected override DbContextOptionsBuilder ConfigureOptions(Func<string> connectionStringFunc, DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.UseSqlServer(connectionStringFunc());
    }
    */

    private static string GetConnectionString()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(configPath))
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            return config.GetConnectionString("DefaultConnection") ??
                   "Server=localhost;Database=Ploch.MyApp;Integrated Security=True;TrustServerCertificate=True";
        }
        return "Server=localhost;Database=Ploch.MyApp;Integrated Security=True;TrustServerCertificate=True";
    }
}
