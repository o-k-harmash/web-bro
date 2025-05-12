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
            .Where(sp => sp.StepProgress?.Completion == 0f)
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
        var stepNavigationVm = new StepNavigationVm
        {
            Id = step.StepId,
            LearningPathId = step.LearningPathId,
            Type = step.Type,
        };

        if (step.Type == StepType.Articles)
        {
            stepNavigationVm.Stage = ArticleStage.Reading;
            // Если шаг является артиклем, возвращаем его навигацию
            return stepNavigationVm;
        }

        var progress = step.StepProgress;
        if (progress == null)
        {
            throw new InvalidOperationException("Прогресс не найден");
        }

        // Вычисляем индекс стейджа
        var index = (int)(progress.Completion / 0.2f);

        // Проверяем, что индекс находится в пределах массива
        if (index < 0 || index >= Challenge.Stages.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(progress.Completion), "Invalid completion value");
        }

        stepNavigationVm.Stage = Challenge.Stages[index];

        return stepNavigationVm;
    }
}
