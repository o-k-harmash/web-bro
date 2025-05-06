using WebBro.DataLayer.EfClasses;

public interface ILearningPathService
{
    int? CompleteStep(int learningPathId, int currentStepId);
    int? GetNextUnfinishedStep(int learningPathId);
    StepDetailsViewModel GetStepDetails(int learningPathId, int stepId);
    List<PreviewViewModel> GetLearningPathsPreview();
    LearningPathDetailsViewModel GetLearningPathDetails(int learningPathId);
}