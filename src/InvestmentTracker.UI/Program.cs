using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InvestmentTracker.Infra.Data;

// Setup Dependency Injection
var builder = Host.CreateApplicationBuilder(args);

// Register the DbContext with the connection string
// This creates the file "investments.db" in your bin/debug folder
builder.Services.AddDbContext<InvestmentContext>(options =>
    options.UseSqlite("Data Source=investments.db"));

var host = builder.Build();

// --- INITIALIZATION ---
// This block ensures the DB is created every time you run the app
using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InvestmentContext>();
    
    // Applies any pending migrations and creates the DB if it doesn't exist
    context.Database.Migrate(); 
    Console.WriteLine("Database verified and ready.");
}

// ... Run your app logic here
host.Run();