using WebBro.DataLayer.EfClasses;

public class ProgressService : IProgressService
{
    public void StartStepIfNeeded(Step step)
    {
        if (step.StepProgress != null)
        {
            return;
        }

        step.StepProgress = new StepProgress
        {
            StepId = step.StepId,
            Completion = 0f,
            UpdatedAt = DateTime.UtcNow,
            Order = step.Order
        };
    }

    public void AddCompleteForStepProgress(Step step, float value)
    {
        var stepProgress = step.StepProgress;

        if (stepProgress != null)
        {
            stepProgress.Completion = Math.Min(1f, stepProgress.Completion + value);
        }
    }

    public int GetPrecentegeFromStepProgress(List<Step> steps)
    {
        return (int)(100 * steps.Average(s =>
            s.StepProgress != null
                ? s.StepProgress.Completion
                : 0f));
    }

    public int GetPrecentegeFromStepProgress(Step step)
    {
        return (int)(100 * (
            step.StepProgress != null
                ? step.StepProgress.Completion
                : 0f));
    }
}