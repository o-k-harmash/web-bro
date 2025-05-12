public class ArticleReadVm
{
    public int Id { get; set; }
    public int LearningPathId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string MarkdownContent { get; set; } = string.Empty;

    public LearningPathPreviewVm? NextStep { get; set; }
}
