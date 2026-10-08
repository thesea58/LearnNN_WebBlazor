using LearnNN_WebBlazor.Components;
using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Models.Ai;
using LearnNN_WebBlazor.Services;
using LearnNN_WebBlazor.Services.Ai;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Service Registration

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DbContextFactory instead of AddDbContext for Blazor Server concurrency safety
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVocabularyService, VocabularyService>();
builder.Services.AddScoped<IMasteryTrackingService, MasteryTrackingService>();
builder.Services.AddScoped<ISrsEngineService, SrsEngineService>();
builder.Services.AddScoped<IStudyService, StudyService>();

// AI Infrastructure & Multi-channel Executors
builder.Services.Configure<AiOptions>(builder.Configuration.GetSection(AiOptions.SectionName));
builder.Services.PostConfigure<AiOptions>(options =>
{
    var envKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    if (!string.IsNullOrWhiteSpace(envKey) && string.IsNullOrWhiteSpace(options.ApiKey))
    {
        options.ApiKey = envKey;
    }
});
builder.Services.AddHttpClient<GeminiApiExecutor>();
builder.Services.AddSingleton<ILenientJsonParser, LenientJsonParser>();
builder.Services.AddSingleton<IPromptBuilder, PromptBuilder>();
builder.Services.AddTransient<ManualBridgeExecutor>();
builder.Services.AddScoped<IAiService, AiService>();

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
