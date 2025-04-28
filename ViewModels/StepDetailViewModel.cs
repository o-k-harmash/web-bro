namespace WebBro.ViewModels;

public class StepDetailViewModel
{
    // Текущий шаг
    public int Id { get; set; }
    public int LearningPathId { get; set; }
    
    public string Title { get; set; } = null!;
    public string MarkdownContent { get; set; } = null!;

    // Следующий шаг (миникарточка)
    public NextStepPreviewViewModel? NextStep { get; set; }

    // Навигация по всем шагам (только ссылки)
    public List<NavbarStepLinkViewModel> NavbarSteps { get; set; } = new();
}