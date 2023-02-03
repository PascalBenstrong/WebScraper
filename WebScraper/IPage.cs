namespace WebScraper;

public interface IPage
{
    Task<bool> GotoAsync(string url);
    Task CloseAsync();
    Task<string> ContentAsync();

    Task<IElement?> FindElementAsync(string selector);

    IAsyncEnumerable<IElement> FindElementsAsyncEnumerable(string selector);

    Task<IEnumerable<IElement>> FindElementsAsync(string selector);

    Task<T> ExecuteAsync<T>(string expression, object? args = null);
    Task ExecuteAsync(string expression, object? args = null);

    Task<IElement?> ExecuteElementAsync(string expression, object? args = null);
    Task RouteAsync(WebScraper.IRouteHandler handler);
    Task UnrouteAsync(Func<string, bool> url);

}

