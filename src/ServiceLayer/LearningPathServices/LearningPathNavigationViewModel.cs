public class NavItemVm
{
    public int LearningPathId { get; set; }
    public int StepId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string StageKey { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public bool IsCurrentPage { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOpen { get; set; }

    public bool HasChildren => Children.Any();
    public List<NavItemVm> Children { get; set; } = new();
}
