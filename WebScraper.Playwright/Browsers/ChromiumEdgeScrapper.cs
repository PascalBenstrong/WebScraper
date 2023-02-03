namespace WebScraper.Playwright.Browsers;

public sealed class ChromiumEdgeScrapper : Scrapper, IScraper
{
    public ChromiumEdgeScrapper(ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<ChromiumEdgeScrapper>(), loggerFactory)
    { }

    public override Task InitializeAsync()
        => InitializeAsync("msedge");

    protected internal override Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, bool headless = true)
    {
        return playwright.Chromium.LaunchAsync(new()
        {
            Env = new Dictionary<string, string>(),
            Headless = headless,
            Channel = "msedge"
        });
    }
}
