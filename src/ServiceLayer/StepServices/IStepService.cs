public interface IStepService
{
    int? CompleteStep(int learningPathId, int currentStepId);
    int? GetNextUnfinishedStep(int learningPathId);
    StepDetailsViewModel GetStepDetails(int learningPathId, int stepId);
}