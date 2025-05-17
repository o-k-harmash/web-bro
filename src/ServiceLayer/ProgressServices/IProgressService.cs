using WebBro.DataLayer.EfClasses;

public interface IProgressService
{
    void StartStepIfNeededV2(Step step);
    void MarkStepAsCompletedV2(Step step);
    void MarkStageAsCompletedV2(Step step, Stage stage);
    void StartStageIfNeededV2(Step step, Stage stage);


    int GetPrecentegeFromStepProgress(List<Step> steps);
    int GetPrecentegeFromStepProgress(Step step);
}