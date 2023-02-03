namespace WebScraper.Playwright;

internal readonly struct RouteWrapper : IRoute
{
    private readonly Microsoft.Playwright.IRoute _route;

    public RouteWrapper(Microsoft.Playwright.IRoute route)
    {
        _route = route;
    }

    public Task AbortAsync()
        => _route.AbortAsync();

    public Task ContinueAsync()
        => _route.ContinueAsync();
}
