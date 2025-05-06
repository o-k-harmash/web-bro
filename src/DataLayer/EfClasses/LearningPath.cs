namespace WebBro.DataLayer.EfClasses;

public class LearningPath
{
    //key
    public int LearningPathId { get; set; }

    //props
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public float Order { get; set; }
    public string ImageUrl { get; set; } = null!;

    //navigation
    public List<Step> Steps { get; set; } = null!;
}