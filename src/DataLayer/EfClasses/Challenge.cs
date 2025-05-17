namespace WebBro.DataLayer.EfClasses;

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

    //navigation
    public int StepId { get; set; }
}