namespace Webbro.V2;

public class StepDetailsViewModel
{
    // Текущий шаг
    public int Id { get; set; }
    public int LearningPathId { get; set; }

    public string Title { get; set; } = null!;
    public string MarkdownContent { get; set; } = null!;

    // Следующий шаг (миникарточка)
    public StepViewModel? NextStep { get; set; }
}