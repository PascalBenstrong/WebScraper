
namespace WebScraper.Playwright;

internal sealed class PlaywrightBrowserContext : IBrowserContext
{
    private Microsoft.Playwright.IBrowserContext? _browser;
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;

    public PlaywrightBrowserContext(Microsoft.Playwright.IBrowserContext browser, ILoggerFactory? loggerFactory = null)
    {
        Guard.IsNotNull(browser);
        _browser = browser;
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory?.CreateLogger<PlaywrightBrowserContext>();
    }
    public async Task<IPage> NewPageAsync()
    {
        _browser ??= ThrowHelper.ThrowObjectDisposedException<Microsoft.Playwright.IBrowserContext>("browser");
        
        _logger?.LogDebug("creating new page");
        var page = await _browser.NewPageAsync();
        _logger?.LogDebug("new page created");

        return new PlaywrightPage(page, _loggerFactory);
    }

    public async Task RouteAsync(WebScraper.IRouteHandler handler)
    {
        _browser ??= ThrowHelper.ThrowObjectDisposedException<Microsoft.Playwright.IBrowserContext>("browser");

        var _handler = _Handler(handler);
        await _browser.RouteAsync(handler.CanRoute, _handler);
        handler.Unlink(() => _browser.UnrouteAsync(handler.CanRoute, _handler));

        static Func<Microsoft.Playwright.IRoute, Task> _Handler(WebScraper.IRouteHandler _handler)
        {
            return x => _handler.HandleRouteAsync(new RouteWrapper(x));
        }
    }

    public Task UnrouteAsync(Func<string, bool> url)
    {
        _browser ??= ThrowHelper.ThrowObjectDisposedException<Microsoft.Playwright.IBrowserContext>("browser");

        return _browser.UnrouteAsync(url);
    }

    public async void Dispose()
    {
        await DisposeAsync();
    }

    public ValueTask DisposeAsync()
    {
        if (_browser == null) return default;

        var b = _browser;
        _browser = null;
        return b.DisposeAsync();
    }
}
