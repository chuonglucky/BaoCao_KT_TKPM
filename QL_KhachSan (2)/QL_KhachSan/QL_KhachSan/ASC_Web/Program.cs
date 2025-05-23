using ASC.DataAccess.Interfaces;
using ASC.DataAccess;
using ASC_Web.Configuration;
using ASC_Web.Data;
using ASC_Web.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Sử dụng các extension methods từ lớp DependencyInjection
builder.Services
    .AddConfig(builder.Configuration)
    .AddMyDependencyGroup();

var app = builder.Build();

// Thực hiện migrate và seed data
try
{
    await SeedDatabaseAsync(app);
}
catch (Exception ex)
{
    var seedLogger = app.Services.GetRequiredService<ILogger<Program>>(); // Renamed to seedLogger
    seedLogger.LogCritical(ex, "Critical error during database seeding. Application will shut down.");
    throw;
}

// Cấu hình HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

var appLogger = app.Services.GetRequiredService<ILogger<Program>>(); // Outer logger
appLogger.LogInformation("Application started successfully. Listening on: {urls}", string.Join(", ", app.Urls));

// Tạo Navigation Cache
using (var scope = app.Services.CreateScope())
{
    var navigationCacheOperations = scope.ServiceProvider.GetRequiredService<INavigationCacheOperations>();
    await navigationCacheOperations.CreateNavigationCacheAsync();
}

app.Run();

async Task SeedDatabaseAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var seedLogger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                seedLogger.LogInformation("Applying migrations: {Migrations}", string.Join(", ", pendingMigrations));
                await context.Database.MigrateAsync();
                seedLogger.LogInformation("Database migrations applied successfully.");
            }
            else
            {
                seedLogger.LogInformation("No pending migrations.");
            }

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var appSettings = services.GetRequiredService<IOptions<ApplicationSettings>>();
            var identitySeeder = services.GetRequiredService<IIdentitySeed>();

            await identitySeeder.Seed(userManager, roleManager, appSettings);
            seedLogger.LogInformation("Identity seeding completed.");
        }
        catch (Exception ex)
        {
            seedLogger.LogCritical(ex, "Error during database migration or seeding.");
            throw;
        }
    }
}