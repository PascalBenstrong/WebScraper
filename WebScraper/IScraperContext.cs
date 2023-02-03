namespace WebScraper;

public interface IScraperContext : IDisposable
{
    Task<IBrowserContext> CreateContextAsync();
}
