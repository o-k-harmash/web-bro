using WebBro.DataLayer.EfClasses;

/// <summary>
/// Доменный сервис работы с прогрессом шагов внутри агрегата Step.
/// Сервис действует строго на уровне в памяти (в рамках aggregate root),
/// не выполняет I/O, и управляет Step.StepProgress как вложенной сущностью.
/// </summary>
public class ProgressService : IProgressService
{
    /// <summary>
    /// Use-case: начать прогресс по шагу, если он ещё не начат.
    /// Используется при первом открытии шага, чтобы инициализировать StepProgress.
    /// </summary>
    public void StartStepIfNeeded(Step step)
    {
        if (step.StepProgress != null)
            return;

        // Вложенная сущность StepProgress создаётся прямо на агрегате Step
        step.StepProgress = new StepProgress
        {
            StepId = step.StepId,
            Completion = StepCompletion.NotStarted,
            UpdatedAt = DateTime.UtcNow,
            Order = step.Order
        };
    }

    /// <summary>
    /// Use-case: добавить прогресс к шагу (например, при завершении шага).
    /// Допустимая норма — от 0 до 1 (1 = завершено).
    /// </summary>
    /// <param name="value">Прирост, от 0.0 до 1.0</param>
    public void AddCompleteForStepProgress(Step step, float value)
    {
        var stepProgress = step.StepProgress;
        if (stepProgress != null)
        {
            stepProgress.Completion = Math.Min(StepCompletion.Completed, stepProgress.Completion + value);
        }
        // Можно логировать попытки завершения без инициализации — если понадобится инвариант
    }

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
}
