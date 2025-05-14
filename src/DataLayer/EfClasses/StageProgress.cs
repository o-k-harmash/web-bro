public class StageProgress
{
    public int StageProgressId { get; set; }
    public string StageKey { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int StepId { get; set; }
    public float Completion { get; set; }
}