using WebBro.DataLayer.EfClasses;

/// <summary>
/// Доменный сервис работы с прогрессом шагов внутри агрегата Step.
/// Сервис действует строго на уровне в памяти (в рамках aggregate root),
/// не выполняет I/O, и управляет Step.StepProgress как вложенной сущностью.
/// </summary>
public class ProgressService : IProgressService
{
    /// <summary>
    /// Use-case: вычислить процент завершения по множеству шагов.
    /// Используется для вычисления общего прогресса по LearningPath.
    /// </summary>
    public int GetPrecentegeFromStepProgress(List<Step> steps)
    {
        return (int)(100 * steps.Average(s =>
            s.StepProgress != null
                ? s.StepProgress.Completion
                : StepCompletion.NotStarted));
    }

    /// <summary>
    /// Use-case: вычислить прогресс по конкретному шагу.
    /// </summary>
    public int GetPrecentegeFromStepProgress(Step step)
    {
        return (int)(100 * (
            step.StepProgress != null
                ? step.StepProgress.Completion
                : StepCompletion.NotStarted));
    }

    public void StartStepIfNeededV2(Step step)
    {
        if (step.StepProgress != null)
        {
            return;
        }

        step.StepProgress = new StepProgress
        {
            StepId = step.StepId,
            Completion = StepCompletion.NotStarted,
            UpdatedAt = DateTime.UtcNow,
        };
        //использовать старт стейдж иф нидид или проверять нет ли созданного прогресса
        step.StepProgress.StageProgresses.Add(new StageProgress
        {
            StepId = step.StepId,

            //использовать сервис навигации
            StageKey = step.StageList
                .First()
                .StageKey,

            UpdatedAt = DateTime.UtcNow,
            Completion = StepCompletion.NotStarted,
        });
    }

    public void MarkStepAsCompletedV2(Step step)
    {
        var stepProgress = step.StepProgress;
        if (stepProgress == null)
        {
            throw new InvalidOperationException("StepProgress is not initialized.");
        }

        //возможно сделать вспомогательный метод обновления или оставить апдейтед на усмотрение базы данных
        stepProgress.Completion = StepCompletion.Completed;
        stepProgress.UpdatedAt = DateTime.UtcNow;

        //или вынести в сервис навиации типо найти ласт стейдж и его прогресс
        var lastStageProgress = stepProgress.StageProgresses
            .OrderBy(sp => sp.Order)
            .LastOrDefault(); //проверка что прогресс действительно последнего шага
        if (lastStageProgress == null)
        {
            throw new InvalidOperationException("No last stage found for this step.");
        }

        lastStageProgress.Completion = StepCompletion.Completed;
        lastStageProgress.UpdatedAt = DateTime.UtcNow;
    }

    public void MarkStageAsCompletedV2(Step step, Stage stage)
    {
        var stepProgress = step.StepProgress;
        if (stepProgress == null)
        {
            throw new InvalidOperationException("StepProgress is not initialized.");
        }
        //видны паттерны нахождение прогресса по стейджу и проверка на наличие
        var stageProgress = stepProgress.StageProgresses.FirstOrDefault(sp => sp.StageKey == stage.StageKey);
        if (stageProgress == null)
        {
            throw new InvalidOperationException("StepProgress is not initialized.");
        }

        if (stageProgress.Completion == StepCompletion.Completed)
        {
            return;
        }

        //логика добавления инлайновая поскольку единственное место где это нужно
        stepProgress.Completion = Math.Min(StepCompletion.Completed, stepProgress.Completion + stage.CompletionPice);

        //видны паттерны обновления прогресса
        stageProgress.Completion = StepCompletion.Completed;
        stageProgress.UpdatedAt = DateTime.UtcNow;
    }

    public void StartStageIfNeededV2(Step step, Stage stage)
    {
        var stepProgress = step.StepProgress;
        if (stepProgress == null)
        {
            throw new InvalidOperationException("StepProgress is not initialized.");
        }

        var stageProgress = stepProgress.StageProgresses.FirstOrDefault(sp => sp.StageKey == stage.StageKey);
        if (stageProgress != null)
        {
            return;
        }

        stepProgress.StageProgresses.Add(new StageProgress
        {
            StepId = step.StepId,
            StageKey = stage.StageKey,
            Completion = StepCompletion.NotStarted,
            UpdatedAt = DateTime.UtcNow,
            //фикс
            Order = stage.Order,
        });
    }
}
