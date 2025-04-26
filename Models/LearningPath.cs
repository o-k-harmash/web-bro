namespace WebBro.Models;

public class Base
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class LearningPath : Base
{
    public string Tag { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}

public class Step : Base
{
    public string LearningPathId { get; set; }
    public float Order { get; set; }
}
