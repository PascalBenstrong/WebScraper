namespace WebScraper;

public interface IRoute
{
    Task AbortAsync();
    Task ContinueAsync();
}
