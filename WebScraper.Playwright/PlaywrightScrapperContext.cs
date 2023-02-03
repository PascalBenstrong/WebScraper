namespace WebScraper.Playwright;

internal sealed class PlaywrightScrapperContext : IScraperContext, IAsyncDisposable
{

    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;

    private Microsoft.Playwright.IBrowser? _browser;
    
    public PlaywrightScrapperContext(Microsoft.Playwright.IBrowser browser, ILoggerFactory? loggerFactory = null)
    {
        Guard.IsNotNull(browser, nameof(browser));
        _browser = browser;
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory?.CreateLogger<PlaywrightScrapperContext>();
    }

    public async Task<IBrowserContext> CreateContextAsync()
    {
        _browser ??= ThrowHelper.ThrowObjectDisposedException<Microsoft.Playwright.IBrowser>("browser");

        _logger?.LogDebug("creating browser context.");

        var context = await _browser.NewContextAsync();

        _logger?.LogDebug("created browser context.");
        return new PlaywrightBrowserContext(context, _loggerFactory);
    }

    public async void Dispose()
    {
        await DisposeAsync();
    }

    public ValueTask DisposeAsync()
    {
        if(_browser is null) return default;

        var browser = _browser;
        _browser = null;
        return browser.DisposeAsync();
    }
}
