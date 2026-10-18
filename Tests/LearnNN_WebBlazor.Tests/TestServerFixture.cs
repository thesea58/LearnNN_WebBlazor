using System.Diagnostics;
using System.Net.Sockets;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Lifecycle manager for the Blazor test web server instance.
/// Automatically starts the host on port 5099 with an isolated SQLite test database if not already running.
/// <para>VN: Quản lý vòng đời máy chủ web Blazor dùng cho kiểm thử tự động trên cổng 5099 với SQLite DB riêng biệt.</para>
/// </summary>
public static class TestServerFixture
{
    #region Constants and Fields

    public const string BaseUrl = "http://localhost:5099";
    private static Process? _serverProcess;
    private static readonly object _syncLock = new();

    #endregion

    #region Public Lifecycle Methods

    /// <summary>
    /// Checks whether port 5099 is listening. If not, spawns the server process with isolated test SQLite db.
    /// </summary>
    public static async Task EnsureServerRunningAsync()
    {
        // A. Check if an external server instance is already running on port 5099
        if (IsPortInUse(5099))
        {
            return;
        }

        lock (_syncLock)
        {
            if (IsPortInUse(5099))
            {
                return;
            }

            // B. Resolve repo root path and prepare test database
            string rootDir = FindSolutionRoot();
            string testDbDir = Path.Combine(rootDir, "Tests", "TestData");
            Directory.CreateDirectory(testDbDir);

            string testDbPath = Path.Combine(testDbDir, "test_vocab.db").Replace('\\', '/');
            string connStr = $"Data Source={testDbPath}";

            // C. Start the host process with test connection string
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{Path.Combine(rootDir, "Source")}\" --urls {BaseUrl} --ConnectionStrings:DefaultConnection=\"{connStr}\"",
                WorkingDirectory = Path.Combine(rootDir, "Source"),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            startInfo.EnvironmentVariables["ConnectionStrings__DefaultConnection"] = connStr;
            startInfo.EnvironmentVariables["ASPNETCORE_URLS"] = BaseUrl;
            startInfo.EnvironmentVariables["TEST_DETERMINISTIC_SEED"] = "true";

            _serverProcess = Process.Start(startInfo);
        }

        // D. Poll until the web application responds with 200 OK
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < TimeSpan.FromSeconds(60))
        {
            try
            {
                var response = await httpClient.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        throw new TimeoutException($"Blazor test server failed to start and respond at {BaseUrl} within 60 seconds.");
    }

    /// <summary>
    /// Terminates the background server process if it was started by this test harness.
    /// </summary>
    public static void StopServer()
    {
        lock (_syncLock)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                try
                {
                    _serverProcess.Kill(entireProcessTree: true);
                }
                catch
                {
                    // Ignore exceptions during process termination
                }
                _serverProcess.Dispose();
                _serverProcess = null;
            }
        }
    }

    #endregion

    #region Internal Helpers

    /// <summary>
    /// Checks TCP port connectivity on localhost.
    /// </summary>
    private static bool IsPortInUse(int port)
    {
        try
        {
            using var client = new TcpClient("127.0.0.1", port);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Traverses parent directories to locate the solution file.
    /// </summary>
    public static string FindSolutionRoot()
    {
        var current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            if (File.Exists(Path.Combine(current, "LearnNN_WebBlazor.slnx")))
            {
                return current;
            }
            current = Directory.GetParent(current)?.FullName;
        }
        return Directory.GetCurrentDirectory();
    }

    #endregion
}

/// <summary>
/// NUnit assembly-level fixture ensuring test web server availability before any tests run.
/// <para>VN: Fixture cấp Assembly cho NUnit nhằm khởi động máy chủ trước khi chạy và dọn dẹp sau khi xong.</para>
/// </summary>
[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public async Task SetUpAsync()
    {
        await TestServerFixture.EnsureServerRunningAsync();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        TestServerFixture.StopServer();
    }
}
