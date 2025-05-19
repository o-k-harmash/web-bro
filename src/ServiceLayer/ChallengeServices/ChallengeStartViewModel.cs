public class ChallengeStartVm
{
    public int Id { get; set; }
    public int LearningPathId { get; set; }

    public string DesktopPreviewImage { get; set; } = string.Empty;
    public string MobilePreviewImage { get; set; } = string.Empty;
    public string BriefMarkdown { get; set; } = string.Empty;
    public string SuggestionMarkdown { get; set; } = string.Empty;
    public string SolutionBaseRepository { get; set; } = string.Empty;
    public LearningPathPreviewVm Step { get; set; } = null!;
    public List<NavItemVm> StepNavs { get; set; } = new();
}
