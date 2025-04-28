namespace WebBro.ViewModels;

public class LearningPathPreviewViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public float Order { get; set; }

    public List<StepPreviewViewModel> Steps { get; set; } = new();
    public List<StepProgressViewModel> StepsProgress { get; set; } = new();
}