namespace Webbro.V2;

public class StepViewModel
{
    public int Id { get; set; }
    public int LearningPathId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tag { get; set; }
    public string ImageUrl { get; set; }
    public int Order { get; set; }
    public int CompletedPercentage { get; set; }
    public string Link { get; set; }
}
