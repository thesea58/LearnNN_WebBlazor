using LearnNN_WebBlazor.Components;
using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Service Registration

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DbContextFactory instead of AddDbContext for Blazor Server concurrency safety
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVocabularyService, VocabularyService>();

var app = builder.Build();

#endregion

#region Database Initialization

// Auto-apply pending migrations on startup (dev convenience; use CI/CD scripts in production)
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = dbFactory.CreateDbContext();
    dbContext.Database.Migrate();
}

#endregion

#region HTTP Pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
#endregion

app.Run();
