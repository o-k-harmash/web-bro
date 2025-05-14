namespace WebBro.DataLayer.EfClasses;

public static class ArticleStage
{
    public const string Reading = "read";
}

public class Article
{
    //key
    public int ArticleId { get; set; }

    public static string[] Stages = new[]
        {
            ArticleStage.Reading,  // 0.0f
        };

    //props
    public string ArticleMarkdown { get; set; } = string.Empty;

    //navigation
    public int StepId { get; set; }
}