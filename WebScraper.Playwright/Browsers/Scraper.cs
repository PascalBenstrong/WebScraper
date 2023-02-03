using System.Linq;
using System.Text;

namespace WebScraper.Playwright.Browsers;

internal class RedirectConsoleLog : IDisposable
{
    private readonly TextWriter? _consoleWriter;
    private readonly TextWriter _streamWriter;
    private readonly StreamReader _streamReader;

    public string? Text { get; private set; }

    public RedirectConsoleLog(): this(new MemoryStream())
    {
    }

    public RedirectConsoleLog(Stream stream)
    {
        _streamWriter = new StreamWriter(stream, Encoding.UTF8);
        _streamReader = new StreamReader(stream);
        _consoleWriter = Console.Out;
        Console.SetOut(Console.Out);
    }

    public void Dispose()
    {
        Console.SetOut(_consoleWriter);
        _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
        Text = _streamReader.ReadToEnd();
        _streamWriter.Flush();
        _streamReader.Dispose();
    }
}

public abstract class Scraper : IScraper, IDisposable
{
    public bool IsInitialized { get; private set; }
    protected readonly ILogger? _logger;
    protected readonly ILoggerFactory? _loggerFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    private IPlaywright? _playwright;

    internal Scraper(ILogger? logger = null, ILoggerFactory? loggerFactory = null)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    private

    protected async Task InitializeAsync(string browser)
    {
        if (IsInitialized) return;

        try
        {
            await _semaphoreSlim.WaitAsync();

            if (IsInitialized) return;
            await _Initialize();
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        async Task _Initialize()
        {
            await Task.Yield();
            _logger?.LogDebug("Initializing Scrapper");

            int exitCode;
            var rd = new RedirectConsoleLog();
            using (rd)
            {
                exitCode = Microsoft.Playwright.Program.Main(new string[] { "install", "--with-deps", browser });
            }

            bool ignoreError = false;
            if(!string.IsNullOrWhiteSpace(rd.Text))
            {
                _logger?.LogError(rd.Text);
                ignoreError = rd.Text!.IndexOf("is already installed", StringComparison.OrdinalIgnoreCase) != -1;
            }

            if (exitCode != 0 && !ignoreError)
            {
                _logger?.LogError("ChromiumScrapper initializing Playwright existed with code {exicode}", exitCode);
                throw new InitializationException();
            }
            _logger?.LogDebug("scrapper Initialized");
            IsInitialized = true;
        }
    }

    protected internal virtual Task<IBrowser> CreateBrowserAsync(IPlaywright playwright, bool headless = true)
    {
        return playwright.Chromium.LaunchAsync(new()
        {
            Env = new Dictionary<string, string>(),
            Headless = headless
        });
    }

    public async Task<IScraperContext> CreateContextAsync(bool headless = true)
    {
        // create browser context

        var playWright = await CreatePlaywright();
        var browser = await CreateBrowserAsync(playWright, headless);

        return new PlaywrightScrapperContext(browser, _loggerFactory);
    }

    private async Task<IPlaywright> CreatePlaywright()
    {
        if (!IsInitialized)
            throw new ScraperNotInitializedException();

        if (_playwright is not null) return _playwright;

        try
        {
            await _semaphoreSlim.WaitAsync();

            if (_playwright is not null) return _playwright;
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return _playwright;
    }

    public abstract Task InitializeAsync();

    public void Dispose()
    {
        var playwright = _playwright;
        _playwright = null;
        playwright?.Dispose();
    }
}
