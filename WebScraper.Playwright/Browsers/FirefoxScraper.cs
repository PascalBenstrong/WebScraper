namespace WebScraper.Playwright.Browsers;

public sealed class FirefoxScraper : Scraper, IScraper
{
    public FirefoxScraper(ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<FirefoxScraper>(), loggerFactory)
    { }

    public override Task InitializeAsync()
        => InitializeAsync("firefox");

    protected internal override Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, bool headless = true)
    {
        return playwright.Firefox.LaunchAsync(new()
        {
            Env = new Dictionary<string, string>(),
            Headless = headless
        });
    }
}
