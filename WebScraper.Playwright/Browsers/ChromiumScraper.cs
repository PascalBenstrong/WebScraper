namespace WebScraper.Playwright.Browsers;

public sealed class ChromiumScraper : Scrapper, IScraper, IDisposable
{
    public ChromiumScraper(ILoggerFactory? loggerFactory = null) : base(loggerFactory?.CreateLogger<ChromiumScraper>(), loggerFactory)
    { }

    public override Task InitializeAsync()
        => InitializeAsync("chromium");

}
