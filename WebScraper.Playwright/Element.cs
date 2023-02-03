namespace WebScraper.Playwright;

internal readonly struct Element : IElement
{
    private readonly IElementHandle _elementHandle;

    public Element(IElementHandle elementHandle)
    {
        _elementHandle = elementHandle;
    }

    public async Task<IElement?> FindElementAsync(string selector)
    {
        var element = await _elementHandle.WaitForSelectorAsync(selector, new() { State = WaitForSelectorState.Attached});

        if (element is null) return null;

        return new Element(element);
    }

    public async IAsyncEnumerable<IElement> FindElementsAsyncEnumerable(string selector)
    {
        var elements = await _elementHandle.QuerySelectorAllAsync(selector);

        foreach (var element in elements)
        {
            yield return new Element(element);
        }
    }

    public async Task<IEnumerable<IElement>> FindElementsAsync(string selector)
    {
        var elements = await _elementHandle.QuerySelectorAllAsync(selector);

        return elements.Select(element => (IElement)new Element(element));
    }


    public async Task<IElement?> FirstElementChildAsync()
    {
        var element = await _elementHandle.EvaluateHandleAsync("node => node.firstElementChild");

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public async Task<IElement?> LastElementChildAsync()
    {
        var element = await _elementHandle.EvaluateHandleAsync("node => node.lastElementChild");

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public async Task<IElement?> NthElementChildAsync(int n)
    {
        var element = await _elementHandle.EvaluateHandleAsync("(node, n) => node.children[n]",n);

        if (element is not IElementHandle elementHandle) return null;

        return new Element(elementHandle);
    }

    public Task<string> InnerHtmlAsync()
    {
        return _elementHandle.InnerHTMLAsync();
    }

    public Task<string> InnerTextAsync()
    {
        return _elementHandle.InnerTextAsync();
    }

    public Task<string> InputValueAsync()
    {
        return _elementHandle.InputValueAsync();
    }

    public Task<string?> GetAttributeAsync(string name) 
    {
        return _elementHandle.GetAttributeAsync(name);
    }

}
