namespace WebBro.DataLayer.EfClasses;

public static class ChallengeStage
{
    public const string Starting = "start";
    public const string Submiting = "submit";
    public const string Reviewing = "review";
    public const string Improving = "improve";
    public const string Finishing = "finish";
}

public class Challenge
{
    //key
    public int ChallengeId { get; set; }

    //props
    public string DesktopPreviewImage { get; set; } = string.Empty;
    public string MobilePreviewImage { get; set; } = string.Empty;
    public string BriefMarkdown { get; set; } = string.Empty;
    public string SuggestionMarkdown { get; set; } = string.Empty;
    public string SolutionBaseRepository { get; set; } = string.Empty;

    public static string[] Stages = new[]
        {
            ChallengeStage.Starting,  // 0.0f
            ChallengeStage.Submiting, // 0.2f
            ChallengeStage.Improving, // 0.4f
            ChallengeStage.Reviewing, // 0.6f
            ChallengeStage.Finishing  // 1.0f
        };

    //navigation
    public int StepId { get; set; }
}