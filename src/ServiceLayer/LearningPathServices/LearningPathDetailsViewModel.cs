public class LearningPathDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Completion { get; set; }
    public List<PreviewViewModel> Steps { get; set; } = new();
}
