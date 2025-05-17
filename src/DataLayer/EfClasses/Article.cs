namespace WebBro.DataLayer.EfClasses;

public class Article
{
    //key
    public int ArticleId { get; set; }

    //props
    public string ArticleMarkdown { get; set; } = string.Empty;

    //navigation
    public int StepId { get; set; }
}