using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace VumbaSoft.ErmanApp;

/// <summary>
/// Shared across every test in the "E2E" collection: installs the Chromium
/// browser binary on first use and keeps a single browser instance alive
/// for the whole test run. Individual tests get an isolated
/// <see cref="IBrowserContext"/>/<see cref="IPage"/> from <see cref="E2ETestBase"/>.
/// </summary>
public class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;

    public IBrowser Browser { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var installExitCode = Microsoft.Playwright.Program.Main(["install", "chromium"]);
        if (installExitCode != 0)
        {
            throw new Exception($"Playwright browser install failed with exit code {installExitCode}.");
        }

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = E2ETestConsts.Headless
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        Playwright.Dispose();
    }
}
