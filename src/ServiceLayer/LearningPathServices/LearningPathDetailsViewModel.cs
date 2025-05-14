public class LearningPathDetailsVm
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Completion { get; set; }
    public bool IsBegining { get; set; }
    public List<LearningPathPreviewVm> Steps { get; set; } = new();
}
