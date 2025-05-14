public class LearningPathPreviewVm
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public float Order { get; set; }
    public int Completion { get; set; }
    public bool IsOpen { get; set; } = false;
}
