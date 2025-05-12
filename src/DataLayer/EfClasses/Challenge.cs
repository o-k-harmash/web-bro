namespace WebBro.DataLayer.EfClasses;

public static class ChallengeStage
{
    public const string Starting = "Start";
    public const string Submiting = "Submit";
    public const string Reviewing = "Review";
    public const string Improving = "Improve";
    public const string Finishing = "Finish";
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
            ChallengeStage.Submiting, // 0.8f
            ChallengeStage.Finishing  // 1.0f
        };

    //navigation
    public int StepId { get; set; }
}