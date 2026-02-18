using Microsoft.EntityFrameworkCore;
using PrintStock.Infrastructure.Persistence;
using PrintStock.Application.Interfaces;
using PrintStock.Application.Services;

// --- CRITICAL FIX: Fixing the Working Directory ---
// This setting forces the use of the 'wwwroot' folder next to the EXE.
var options = new WebApplicationOptions
{
    Args = args,
    // Set the application's running directory as the root directory:
    ContentRootPath = AppContext.BaseDirectory,
    // Explicitly specify the location of static files (Blazor):
    WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot") 
};

var builder = WebApplication.CreateBuilder(options);

// --- Forcing the port to 5000 ---
builder.WebHost.UseUrls("http://localhost:5000");

// 1. Database Registration (SQLite)
// We can provide the full path to ensure the database is created next to the EXE, or leave it like this.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "inventory.db")}"));

// 2. Interface and Service Registrations
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IFilamentService, FilamentService>();

// 3. Controller and Swagger Support
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- AUTO DATABASE CREATION (Auto-Migration) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Creates the tables
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

// 4. HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// We can disable HTTPS redirection in production (not required for localhost)
// app.UseHttpsRedirection(); 

// --- CRITICAL: Serving Blazor WASM Files ---
app.UseBlazorFrameworkFiles();
app.UseStaticFiles(); // This will now use the WebRootPath defined above!

app.UseRouting();
app.UseCors("BlazorPolicy");
app.UseAuthorization();

// API Routes
app.MapControllers();

// --- Redirect to Blazor on page refresh ---
app.MapFallbackToFile("index.html");

// 5. AUTO OPEN BROWSER
app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        System.Diagnostics.Process.Start("explorer", "http://localhost:5000");
    }
    catch (Exception)
    {
        Console.WriteLine("Browser could not be opened. Please navigate manually to: http://localhost:5000");
    }
});

app.Run();
