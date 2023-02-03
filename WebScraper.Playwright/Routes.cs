namespace WebScraper.Playwright;

public static class Routes
{
    public static IRouteHandler Create(Func<string, bool> urlHandler, Func<IRoute, Task> handler)
        => new CallbackRoute(urlHandler, handler);
}
