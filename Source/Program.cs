using LearnNN_WebBlazor.Components;
using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Models.Ai;
using LearnNN_WebBlazor.Services;
using LearnNN_WebBlazor.Services.Ai;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Service Registration

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Retain disconnected circuits for 10 minutes to protect active mobile study sessions during brief backgrounding or network switches
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10);
        options.DisconnectedCircuitMaxRetained = 200;
    });

// Support Reverse Proxy headers (Cloudflare Tunnel, Caddy, Nginx, Docker)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

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
    // 1. Resolve API key if not configured or empty in appsettings
    if (string.IsNullOrWhiteSpace(options.ApiKey))
    {
        var candidates = new[] { "GEMINI_API_KEY", "GOOGLE_API_KEY", "GOOGLE_AI_KEY", "AI_API_KEY" };

        foreach (var keyName in candidates)
        {
            var val = Environment.GetEnvironmentVariable(keyName);
            if (string.IsNullOrWhiteSpace(val) && OperatingSystem.IsWindows())
            {
                val = Environment.GetEnvironmentVariable(keyName, EnvironmentVariableTarget.User)
                      ?? Environment.GetEnvironmentVariable(keyName, EnvironmentVariableTarget.Machine);
            }

            if (!string.IsNullOrWhiteSpace(val))
            {
                options.ApiKey = val.Trim('"', '\'', ' ');
                break;
            }
        }

        // Check local .env file fallback
        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            var envFiles = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),
                Path.Combine(AppContext.BaseDirectory, ".env"),
                Path.Combine(Directory.GetCurrentDirectory(), "..", ".env")
            };

            foreach (var envFile in envFiles)
            {
                if (File.Exists(envFile))
                {
                    try
                    {
                        foreach (var line in File.ReadAllLines(envFile))
                        {
                            var trimmed = line.Trim();
                            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#")) continue;
                            var parts = trimmed.Split('=', 2);
                            if (parts.Length == 2)
                            {
                                var k = parts[0].Trim();
                                var v = parts[1].Trim('"', '\'', ' ');
                                if (candidates.Contains(k, StringComparer.OrdinalIgnoreCase) ||
                                    k.Equals("Ai__ApiKey", StringComparison.OrdinalIgnoreCase) ||
                                    k.Equals("Ai:ApiKey", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!string.IsNullOrWhiteSpace(v))
                                    {
                                        options.ApiKey = v;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore file read error on local .env
                    }
                }
                if (!string.IsNullOrWhiteSpace(options.ApiKey)) break;
            }
        }
    }
    else
    {
        // Sanitize quotes if set via appsettings or config
        options.ApiKey = options.ApiKey.Trim('"', '\'', ' ');
    }

    // 2. If an API key is present and mode was default Manual, auto-promote to Hybrid so automated API calls take effect
    if (!string.IsNullOrWhiteSpace(options.ApiKey) && options.Mode == AiMode.Manual)
    {
        var configuredMode = builder.Configuration.GetSection("Ai")["Mode"];
        if (string.IsNullOrWhiteSpace(configuredMode) || configuredMode.Equals("Manual", StringComparison.OrdinalIgnoreCase))
        {
            options.Mode = AiMode.Hybrid;
        }
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

// Ensure parent directory exists for SQLite database file (crucial for Docker persistent volumes)
var rawConnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
var connMatch = System.Text.RegularExpressions.Regex.Match(rawConnStr, @"Data Source=([^;]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
if (connMatch.Success)
{
    var dbFile = connMatch.Groups[1].Value.Trim();
    var dir = Path.GetDirectoryName(dbFile);
    if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
    {
        Directory.CreateDirectory(dir);
    }
}

// Auto-apply pending migrations on startup (dev convenience; use CI/CD scripts in production)
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = dbFactory.CreateDbContext();
    dbContext.Database.Migrate();
}

#endregion

#region HTTP Pipeline

app.UseForwardedHeaders();

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
