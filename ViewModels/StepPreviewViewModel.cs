namespace WebBro.ViewModels;

public class StepPreviewViewModel
{
    public int Id { get; set; }
    public int LearningPathId { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public float Order { get; set; }

    public bool IsCompleted { get; set; }
}