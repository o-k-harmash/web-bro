namespace WebBro.DataLayer.EfClasses;

public class Step
{
    //key
    public int StepId { get; set; }

    //props
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public float Order { get; set; }
    public string ImageUrl { get; set; } = null!;

    //navigation
    public int LearningPathId { get; set; }
    public StepProgress? StepProgress { get; set; }
}