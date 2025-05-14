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
    //стейджи часть шага пусть получаются и хранятся внутри в зависимости от типа
    // private string[]? _stages { get; set; } = null;
    public string[] Stages { get; set; } = new string[] {
                    "start",
                    "submit",
                    "review",
                    "improve",
                    "finish"};
    // {
    //     get
    //     {
    //         if (_stages != null)
    //             return _stages;

    //         _stages = Type switch
    //         {
    //             StepType.Articles => Article.Stages,
    //             StepType.Challenges =>,
    //             _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
    //         };

    //         return _stages;
    //     }
    //     set
    //     {
    //         _stages = value;
    //     }
    // }
    //navigation
    public int LearningPathId { get; set; }
    public StepProgress? StepProgress { get; set; }
    public List<StageProgress> StageProgresses { get; set; } = new List<StageProgress>();
    public Article? Article { get; set; }
    public Challenge? Challenge { get; set; }
}