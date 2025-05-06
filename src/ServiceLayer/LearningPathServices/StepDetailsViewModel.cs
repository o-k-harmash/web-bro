public class StepDetailsViewModel
{
    public int Id { get; set; }
    public int LearningPathId { get; set; }
    public string Title { get; set; } = null!;
    public string MarkdownContent { get; set; } = null!;
    public PreviewViewModel? NextStep { get; set; }
}
