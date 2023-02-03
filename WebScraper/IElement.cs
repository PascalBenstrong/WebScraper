namespace WebScraper;

public interface IElement
{
    Task<IElement?> FindElementAsync(string selector);

    IAsyncEnumerable<IElement> FindElementsAsyncEnumerable(string selector);

    Task<IEnumerable<IElement>> FindElementsAsync(string selector);

    Task<IElement?> FirstElementChildAsync();

    Task<IElement?> LastElementChildAsync();

    Task<IElement?> NthElementChildAsync(int n);

    Task<string> InnerHtmlAsync();

    Task<string> InnerTextAsync();

    Task<string> InputValueAsync();

    Task<string?> GetAttributeAsync(string name);
}
