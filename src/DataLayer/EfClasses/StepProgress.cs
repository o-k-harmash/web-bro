namespace WebBro.DataLayer.EfClasses;

public class StepProgress
{
    //key
    public int StepProgressId { get; set; }

    //props
    public float Completion { get; set; }
    public DateTime UpdatedAt { get; set; }

    //navigation
    public int StepId { get; set; }
}