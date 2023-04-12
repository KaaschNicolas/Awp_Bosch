using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Entityframework_MSSQL_Test.DataAccess;
using Entityframework_MSSQL_Test.Models;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserContext>(
                    options => options.UseSqlServer(@"Data Source=localhost;Initial Catalog=TestDB;User ID=sa;Password=Nicolas!1234;TrustServerCertificate=True"));
            })
            .Build();

        var db = ActivatorUtilities.CreateInstance<UserContext>(host.Services);

        if (db.Users.Count() == 0)
        {
            var file = File.ReadAllText("mockData.json");
            var mockData = JsonSerializer.Deserialize<List<User>>(file);
            db.AddRange(mockData);
            db.SaveChanges();
            Console.WriteLine($"Uploaded {db.Users.Count()} DB Records.");
        }

        var users = db.Users
            .Include(a => a.Emails)
            .ToList();

        if(users.Count != 0)
        {
            Console.WriteLine("Not Empty");
        } else
        {
            Console.WriteLine("Empty");

        }
        
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
    }
        
}

public class Startup
{
    
}