namespace WebScraper;

public interface IRouteHandler : IDisposable, IAsyncDisposable
{
    bool CanRoute(string url);
    void Unlink(Func<Task> unlink);
    Task HandleRouteAsync(IRoute route);
}
