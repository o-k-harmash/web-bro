
public interface ILearningPathNavigationService
{
    List<NavItemVm> GetLearningPathNavigation(int learningPathId, int stepId, string stageKey);
    StepNavigationVm? GetStepToContinue(int learningPathId);
}