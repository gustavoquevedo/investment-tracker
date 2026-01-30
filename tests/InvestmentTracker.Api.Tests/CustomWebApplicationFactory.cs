using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using InvestmentTracker.Infra.Data;

namespace InvestmentTracker.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureServices(services =>
        {
            // Remove ALL DbContext-related registrations
            services.RemoveAll(typeof(DbContextOptions<InvestmentContext>));
            services.RemoveAll(typeof(InvestmentContext));

            // Create an in-memory SQLite connection that stays open
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Add DbContext with SQLite in-memory connection
            services.AddDbContext<InvestmentContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            // Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InvestmentContext>();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
