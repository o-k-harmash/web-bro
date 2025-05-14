using WebBro.DataLayer.EfClasses;

public interface IProgressService
{
    void MarkStageAsCompleted(Step step, string stageKey);
    void StartStageIfNeeded(Step step, string stageKey);
    void MarkStepAsCompleted(Step step);
    void AddCompleteForStepProgress(Step step, float value);
    void StartStepIfNeeded(Step step);
    int GetPrecentegeFromStepProgress(List<Step> steps);
    int GetPrecentegeFromStepProgress(Step step);
}