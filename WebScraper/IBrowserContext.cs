namespace WebScraper;

public interface IBrowserContext : IDisposable, IAsyncDisposable
{
    Task<IPage> NewPageAsync();
    Task RouteAsync(WebScraper.IRouteHandler handler);
    Task UnrouteAsync(Func<string, bool> url);
}
