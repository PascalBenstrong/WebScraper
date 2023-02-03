namespace WebScraper.Playwright.Browsers;

public sealed class ChromeScraper : Scrapper, IScraper
{
    public ChromeScraper(ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<ChromeScraper>(), loggerFactory)
    { }

    public override Task InitializeAsync()
        => InitializeAsync("chrome");

    protected internal override Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, bool headless = true)
    {
        return playwright.Chromium.LaunchAsync(new()
        {
            Env = new Dictionary<string, string>(),
            Headless = headless,
            Channel = "chrome"
        });
    }
}
