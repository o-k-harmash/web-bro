namespace WebBro.DataLayer.EfClasses;

public static class StepType
{
    public const string Articles = "Articles";
    public const string Challenges = "Challenges";
}

public class Step
{
    //key
    public int StepId { get; set; }

    //props
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public float Order { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Type { get; set; } = string.Empty;

    //navigation
    public int LearningPathId { get; set; }
    public StepProgress? StepProgress { get; set; }
    public Article? Article { get; set; }
    public Challenge? Challenge { get; set; }
}