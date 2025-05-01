namespace Webbro.V2;

public class LearningPathViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tag { get; set; }
    public string ImageUrl { get; set; }

    public string ContinueLink { get; set; }

    public int CompletedPercentage { get; set; }

    public string Link { get; set; }

    public List<StepViewModel> Steps { get; set; } = new();
}
