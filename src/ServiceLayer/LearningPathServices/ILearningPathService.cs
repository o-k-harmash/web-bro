
public interface ILearningPathService
{
    StepNavigationVm OpenStep(int learningPathId, int stepId);
    StepNavigationVm? MarkStepAsCompletedAndProceed(int learningPathId, int currentStepId);
    StepNavigationVm StartLearningPath(int learningPathId);
    List<LearningPathPreviewVm> GetLearningPathsPreview();
    LearningPathDetailsVm GetLearningPathDetails(int learningPathId);
}