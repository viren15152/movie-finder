using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieFinder.Data;  // ✅ Ensure correct namespace
using MovieFinder.Services; // ✅ Ensure correct namespace

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Load database connection string from .env
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing! Ensure DB_CONNECTION is set in .env.");
}

// Register Database Context with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ✅ Register TMDb Service
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



