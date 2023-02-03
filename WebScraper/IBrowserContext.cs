namespace WebScraper;

public interface IBrowserContext : IDisposable, IAsyncDisposable
{
    public Task<IPage> NewPageAsync();
}
