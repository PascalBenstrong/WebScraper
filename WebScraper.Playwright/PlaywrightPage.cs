namespace WebScraper.Playwright;

internal sealed class PlaywrightPage : IPage
{
    private readonly Microsoft.Playwright.IPage _page;
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;

    public PlaywrightPage(Microsoft.Playwright.IPage page, ILoggerFactory? loggerFactory = null)
    {
        _page = page;
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory?.CreateLogger<PlaywrightPage>();
    }

    public async Task CloseAsync()
    {
        await _page.CloseAsync();
    }

    public async Task<bool> GotoAsync(string url)
    {
        try
        {
            _logger?.LogDebug("navigating to {url}", url);
            var response = await _page.GotoAsync(url);

            _logger?.LogDebug("navigation status : {status}", response?.Status);

            // navigated to the same page or about:blank
            if (response is null) return true;

            return response.Status >= 200 && response.Status <= 299;

        }
        catch(Exception e)
        {
            _logger?.LogError(e, string.Empty);
            return false;
        }

    }

    public async Task<IElement?> FindElementAsync(string selector)
    {
        var element = await _page.WaitForSelectorAsync(selector, new() { State = WaitForSelectorState.Attached});
        
        if (element is null) return null;

        return new Element(element);
    }

    public async Task<IElement?> FindFirstElementAsync(string selector)
    {
        var element = await _page.Locator(selector).First.EvaluateHandleAsync("node => node");

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public async Task<IElement?> FindLastElementAsync(string selector)
    {
        var element = await _page.Locator(selector).Last.EvaluateHandleAsync("node => node");

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public async Task<IElement?> FindNthElementAsync(string selector, int n)
    {
        var element = await _page.Locator(selector).Nth(n).EvaluateHandleAsync("node => node");

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public async IAsyncEnumerable<IElement> FindElementsAsyncEnumerable(string selector)
    {
        var elements = await _page.QuerySelectorAllAsync(selector);

        foreach(var element in elements)
        {
            yield return new Element(element);
        }
    }

    public async Task<IEnumerable<IElement>> FindElementsAsync(string selector)
    {
        var elements = await _page.QuerySelectorAllAsync(selector);

        return elements.Select(element => (IElement)new Element(element));
    }

    public Task<string> ContentAsync()
    {
        return _page.ContentAsync();
    }

    public Task<T> ExecuteAsync<T>(string expression, object? args = null)
    {
        return _page.EvaluateAsync<T>(expression, args);
    }

    public Task ExecuteAsync(string expression, object? args = null)
    {
        return _page.EvaluateAsync(expression, args);
    }

    public async Task<IElement?> ExecuteElementAsync(string expression, object? args = null)
    {
        var handle = await _page.EvaluateHandleAsync(expression, args);

        if (handle is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }
}
