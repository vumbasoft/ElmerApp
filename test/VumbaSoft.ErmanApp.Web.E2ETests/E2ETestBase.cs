using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace VumbaSoft.ErmanApp;

[Collection(E2ETestCollection.Name)]
public abstract class E2ETestBase : IAsyncLifetime
{
    protected IPage Page { get; private set; } = null!;

    private readonly PlaywrightFixture _fixture;
    private IBrowserContext _context = null!;

    protected E2ETestBase(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _context = await _fixture.Browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = E2ETestConsts.BaseUrl,
            IgnoreHTTPSErrors = true
        });
        Page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.CloseAsync();
    }
}
