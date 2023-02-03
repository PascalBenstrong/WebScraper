namespace WebScraper.Playwright.Browsers;

public sealed class WebkitScraper : Scraper, IScraper
{
    public WebkitScraper(ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<WebkitScraper>(), loggerFactory)
    { }

    public override Task InitializeAsync()
        => InitializeAsync("webkit");

    protected internal override Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, bool headless = true)
    {
        return playwright.Webkit.LaunchAsync(new()
        {
            Env = new Dictionary<string, string>(),
            Headless = headless
        });
    }

}
