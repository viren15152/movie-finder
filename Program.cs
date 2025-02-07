using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieFinder.Data;  
using MovieFinder.Services; 

// Only load .env file in local development
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    DotEnv.Load();
}

var builder = WebApplication.CreateBuilder(args);

// Load database connection string from environment variable (Render will handle this)
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing! Ensure DB_CONNECTION is set in environment variables.");
}

// Register Database Context with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<TmdbService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Redirect root URL to Movie Search page
app.MapGet("/", () => Results.Redirect("/Movie/Search"));

app.Run();




