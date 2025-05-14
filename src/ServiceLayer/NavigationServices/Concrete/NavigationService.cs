using WebBro.DataLayer.EfClasses;

/// <summary>
/// Доменный сервис для навигации по шагам в рамках агрегата LearningPath.
/// Обрабатывает только in-memory навигационные сценарии без обращений к БД.
/// </summary>
public class NavigationService : INavigationService
{
    public NavigationService() { }

    /// <summary>
    /// Use-case: найти первый незавершённый шаг в пути.
    /// Применяется при сценарии "продолжить обучение".
    /// </summary>
    public Step? FindContinueStepInPath(LearningPath learningPath)
    {
        return learningPath.Steps
            .OrderBy(sp => sp.Order)
            .Where(sp => sp.StepProgress?.Completion < StepCompletion.Completed)
            .FirstOrDefault();
    }

    /// <summary>
    /// Use-case: найти следующий шаг по порядку после текущего.
    /// Применяется при завершении шага для перехода к следующему.
    /// </summary>
    public Step? FindNextStepInPath(LearningPath learningPath, Step step)
    {
        return learningPath.Steps
            .Where(sp => sp.Order > step.Order)
            .OrderBy(sp => sp.Order)
            .FirstOrDefault();
    }

    /// <summary>
    /// Use-case: найти следующий этап по порядку после текущего.
    /// Применяется при завершении этапа для перехода к следующему.
    /// </summary>
    public string FindNextStage(Step step, string currentStage)
    {
        var stages = step.Stages;
        if (stages == null || stages.Length == 0)
        {
            throw new InvalidOperationException("Stage keys not found or empty");
        }

        Console.WriteLine($"Current stage: {currentStage}");
        stages.ToList().ForEach(s => Console.WriteLine($" stage: {s}"));
        var currentIndex = Array.IndexOf(stages, currentStage);

        if (currentIndex < 0 || currentIndex >= stages.Length - 1)
        {
            throw new Exception("Нет следующего этапа для шага"); // Нет следующего этапа
        }

        return stages[currentIndex + 1];
    }

    public string FindFirstStage(Step step)
    {
        return step.Stages[0];
    }

    public StageProgress? FindStageProgressById(Step step, string stageKey)
    {
        //фикс скапиталайзами
        var stageProgress = step.StageProgresses.FirstOrDefault(sp => string.Equals(sp.StageKey, stageKey, StringComparison.OrdinalIgnoreCase));
        if (stageProgress == null)
        {
            return null;
        }
        return stageProgress;
    }

    /// <summary>
    /// Use-case: получить конкретный шаг по ID.
    /// Используется при открытии конкретного шага пользователем.
    /// </summary>
    public Step FindStepInPathById(LearningPath learningPath, int stepId)
    {
        return learningPath.Steps
            .First(sp => sp.StepId == stepId);
    }

    /// <summary>
    /// Use-case: получить первый шаг в пути.
    /// Применяется при инициализации нового пути или расчёте прогресса.
    /// </summary>
    public Step FindFirstInPath(LearningPath learningPath)
    {
        return learningPath.Steps
            .OrderBy(sp => sp.Order)
            .First();
    }

    /// <summary>
    /// Use-case: получить навигацию по шагу.
    /// </summary>
    /// <param name="learningPath"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public StepNavigationVm GetStepNavigationVm(LearningPath learningPath, Step step)
    {
        //можно переписать под стейдж прогресс модель и универсально с помощью степ стейдж поля
        //для теста сортирую по айдишнику
        var lastStageProgress = step.StageProgresses.OrderBy(sp => sp.StageProgressId).LastOrDefault();
        if (lastStageProgress == null)
        {
            throw new InvalidOperationException("Прогресс не создан");
        }
        return new StepNavigationVm
        {
            Id = step.StepId,
            LearningPathId = step.LearningPathId,
            Type = step.Type,
            Stage = lastStageProgress.StageKey
        }; ;
    }
}
