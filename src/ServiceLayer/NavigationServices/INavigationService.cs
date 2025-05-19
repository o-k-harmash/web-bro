using WebBro.DataLayer.EfClasses;

public interface INavigationService
{
    Stage FindStageByKeyV2(Step step, string stageKey);
    Stage FindNextStageInStepV2(Step step, Stage prevStage);

    Step FindFirstInPath(LearningPath learningPath);
    Step? FindContinueStepInPath(LearningPath learningPath);
    Step? FindNextStepInPath(LearningPath learningPath, Step step);
    Step FindStepInPathById(LearningPath learningPath, int stepId);
    StepNavigationVm GetStepNavigationVm(LearningPath learningPath, Step step);
    List<NavItemVm> GetStepNavs();
}