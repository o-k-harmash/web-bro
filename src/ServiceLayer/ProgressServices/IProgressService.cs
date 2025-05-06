using WebBro.DataLayer.EfClasses;

public interface IProgressService
{
    void AddCompleteForStepProgress(Step step, float value);
    void StartStepIfNeeded(Step step);
    int GetPrecentegeFromStepProgress(List<Step> steps);
    int GetPrecentegeFromStepProgress(Step step);
}