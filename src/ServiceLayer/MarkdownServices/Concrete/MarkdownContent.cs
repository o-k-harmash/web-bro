using Markdig;
using Microsoft.AspNetCore.Hosting;

public class MarkdownService : IMarkdownService
{
    private readonly IWebHostEnvironment _env;

    public MarkdownService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public string GetMarkdownContent(string fileName)
        => Markdown.ToHtml(System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, fileName)));
}