using WebBro.DataLayer.EfClasses;

public interface INavigationService
{
    StageProgress? FindStageProgressById(Step step, string stageKey);
    string FindNextStage(Step step, string currentStage);
    Step FindFirstInPath(LearningPath learningPath);
    Step? FindContinueStepInPath(LearningPath learningPath);
    Step? FindNextStepInPath(LearningPath learningPath, Step step);
    Step FindStepInPathById(LearningPath learningPath, int stepId);
    StepNavigationVm GetStepNavigationVm(LearningPath learningPath, Step step);
}