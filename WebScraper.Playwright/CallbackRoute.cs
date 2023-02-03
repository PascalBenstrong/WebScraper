using System.Collections.Concurrent;

namespace WebScraper.Playwright;

internal readonly struct CallbackRoute : WebScraper.IRouteHandler
{
    private readonly Func<string, bool> _urlHandler;
    private readonly Func<IRoute, Task> _handler;
    private readonly ConcurrentQueue<Func<Task>> _unlinks;

    public CallbackRoute(Func<string, bool> urlHandler, Func<IRoute, Task> handler)
    {
        _urlHandler = urlHandler;
        _handler = handler;
        _unlinks = new();
    }

    public bool CanRoute(string url)
        => _urlHandler(url);

    public async void Dispose()
    {
        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_unlinks.IsEmpty) return;

        List<Task> tasks = new(_unlinks.Count);

        while (_unlinks.TryDequeue(out var f))
        {
            tasks.Add(f());
        }

        await Task.WhenAll(tasks);
    }

    public Task HandleRouteAsync(IRoute route)
        => _handler(route);

    public void Unlink(Func<Task> unlink)
    {
        _unlinks.Enqueue(unlink);
    }
}
