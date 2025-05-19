
public interface ILearningPathService
{
    List<NavItemVm> GetLearningPathNavigation(int learningPathId, int stepId, string stageKey);
    StepNavigationVm OpenStep(int learningPathId, int stepId);
    StepNavigationVm? GetStepToContinue(int learningPathId);
    StepNavigationVm? MarkStepAsCompletedAndProceed(int learningPathId, int currentStepId);
    StepNavigationVm StartLearningPath(int learningPathId);
    List<LearningPathPreviewVm> GetLearningPathsPreview();
    LearningPathDetailsVm GetLearningPathDetails(int learningPathId);
}