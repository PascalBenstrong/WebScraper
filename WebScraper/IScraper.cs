using System.Net;

namespace WebScraper;

public interface IScraper
{
    bool IsInitialized { get; }
    Task InitializeAsync();
    Task<IScraperContext> CreateContextAsync(bool headless = true);
}

public interface IScapperProxy : IWebProxy
{

}
