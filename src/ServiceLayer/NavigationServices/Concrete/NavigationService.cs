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

    public List<NavItemVm> GetStepNavs()
    {
        return new List<NavItemVm>
{
    // 1. Обычный шаг без стейджей, завершён
    new NavItemVm
    {
        Title = "Intro to Frontend Mentor",
        LearningPathId = 0,
        IsOpen = true,
        IsCurrentPage = false,
        StepId = 1,
        Type = "Articles",
        IsCompleted = true
    },

    // 2. Текущий шаг без стейджей, не завершён
    new NavItemVm
    {
        Title = "Intro to Frontend Mentor",
        LearningPathId = 1,
        IsOpen = true,
        IsCurrentPage = true,
        StepId = 1,
        Type = "Articles",
        StageKey = "read",
        IsCompleted = false
    },

    // 3. Шаг со стейджами, завершённый
    new NavItemVm
    {
        Title = "QR Code Component",
        LearningPathId = 3,
        IsOpen = false,

        IsCurrentPage = false,

        StepId = 10,
        Type = "",

        IsCompleted = false,
        Children = new List<NavItemVm>
        {
           new NavItemVm
            {
                Title = "Intro to Frontend Mentor",
                LearningPathId = 0,
                IsOpen = true,
                IsCurrentPage = false,
                StepId = 1,
                Type = "Articles",
                IsCompleted = true
            },
            new NavItemVm
            {
                Title = "Submit solution",
                StageKey = "submit",
                IsCurrentPage = false,
                IsCompleted = true,
                Type = "Articles",
                IsOpen = true
            }
        }
    },

    // 4. Текущий шаг со стейджами, текущая страница — один из стейджей
    new NavItemVm
    {
        Title = "Blog preview card",
        LearningPathId = 4,
        IsOpen = true,

        IsCurrentPage = true,

        StepId = 4,
        Type = "",

        IsCompleted = false,
        Children = new List<NavItemVm>
        {
            new NavItemVm

            {
                Title = "Start challenge",
                StageKey = "start",
                IsCurrentPage = false,
                Type = "challenge",
                IsCompleted = true,
                IsOpen = true
            },
            new NavItemVm
            {
                Title = "Submit solution",
                StageKey = "submit",
                IsCurrentPage = true,
                Type = "challenge",
                IsCompleted = false,
                IsOpen = true
            }
        }
    }
        };
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
        var lastStageProgress = step.StageProgresses
        //фикс
            .OrderBy(sp => sp.Order)
            .LastOrDefault();
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

    public Stage FindStageByKeyV2(Step step, string stageKey)
    {
        var stage = step.StageList.FirstOrDefault(s => string.Equals(s.StageKey, stageKey, StringComparison.OrdinalIgnoreCase));
        if (stage == null)
        {
            throw new InvalidOperationException("Stage not found");
        }
        return stage;
    }

    public Stage FindNextStageInStepV2(Step step, Stage prevStage)
    {
        var nextStage = step.StageList
            .OrderBy(s => s.Order)
            .FirstOrDefault(s => s.Order > prevStage.Order);
        if (nextStage == null)
        {
            throw new InvalidOperationException("Stage not found");
        }
        return nextStage;
    }
}
