using System.ComponentModel.DataAnnotations.Schema;

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
    
    [NotMapped]
    private List<Stage>? _stageList { get; set; } = null;
    [NotMapped]
    public List<Stage> StageList
    {
        get
        {
            if (_stageList != null)
                return _stageList;

            _stageList = Type switch
            {
                StepType.Articles => new List<Stage>
                    {
                        new Stage
                        {
                            StageId = 1,
                            StageKey = "read",
                            Order = 0,
                            CompletionPice = 1f
                        }
                    },
                StepType.Challenges => new List<Stage>
                    {
                        new Stage
                        {
                            StageId = 2,
                            StageKey = "start",
                            Order = 0,
                            CompletionPice = 0.2f
                        },
                        new Stage
                        {
                            StageId = 3,
                            StageKey = "submit",
                            Order = 1,
                            CompletionPice = 0.2f
                        },
                        new Stage
                        {
                            StageId = 4,
                            StageKey = "review",
                            Order = 2,
                            CompletionPice = 0.2f
                        },
                        new Stage
                        {
                            StageId = 5,
                            StageKey = "improve",
                            Order = 3,
                            CompletionPice = 0.2f
                        },
                        new Stage
                        {
                            StageId = 6,
                            StageKey = "finish",
                            Order = 4,
                            CompletionPice = 0.2f
                        }
                    },
                _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
            };

            return _stageList;
        }
    }

    //navigation
    public int LearningPathId { get; set; }
    public StepProgress? StepProgress { get; set; }
    public List<StageProgress> StageProgresses { get; set; } = new List<StageProgress>();
    public Article? Article { get; set; }
    public Challenge? Challenge { get; set; }
}