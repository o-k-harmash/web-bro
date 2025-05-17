using DataLayer;
using WebBro.DataLayer.EfClasses;

public class LearningPathService : ILearningPathService
{
    private readonly AppDbContext _ctx;
    private readonly IProgressService _progressService;
    private readonly INavigationService _navigationService;

    public LearningPathService(AppDbContext ctx, IProgressService progressService, INavigationService navigationService)
    {
        _ctx = ctx;
        _progressService = progressService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Use-case: Получить детальную модель пути обучения для UI.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <returns>ViewModel с шагами, прогрессом и описанием</returns>
    public LearningPathDetailsVm GetLearningPathDetails(int learningPathId)
    {
        // Загружаем агрегат LearningPath с шагами и прогрессом
        var learningPath = _ctx.LearningPaths
            .IncludeStepsWithProgress()
            .SingleOrDefault(lp => lp.LearningPathId == learningPathId);

        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь не найден");
        }

        //легаси фича удалить так как кнопка старт с акшином старта
        // Находим первый шаг и инициируем его прогресс при первом посещении
        var firstStep = _navigationService.FindFirstInPath(learningPath);

        // _progressService.StartStepIfNeeded(firstStep);
        // _ctx.SaveChanges();

        // Формируем модель для вьюхи
        return new LearningPathDetailsVm
        {
            Id = learningPath.LearningPathId,
            Title = learningPath.Title,
            Description = learningPath.Description,
            ImageUrl = learningPath.ImageUrl,

            IsBegining = firstStep.StepProgress == null,

            // Общий процент завершения по всем шагам
            Completion = _progressService.GetPrecentegeFromStepProgress(learningPath.Steps),

            // Отображение всех шагов с индивидуальным прогрессом
            Steps = learningPath.Steps
                .OrderBy(sp => sp.Order)
                .Select(sp => new LearningPathPreviewVm
                {
                    Id = sp.StepId,
                    Title = sp.Title,
                    Description = sp.Description,
                    ImageUrl = sp.ImageUrl,
                    IsOpen = sp.StepProgress != null,
                    Completion = _progressService.GetPrecentegeFromStepProgress(sp)
                })
                .ToList()
        };
    }

    /// <summary>
    /// Use-case: Получить список всех путей обучения с кратким описанием и прогрессом.
    /// </summary>
    /// <returns>Список Preview моделей для вывода на главной</returns>
    public List<LearningPathPreviewVm> GetLearningPathsPreview()
    {
        // Загружаем все пути обучения с шагами и прогрессом
        var learningPathList = _ctx.LearningPaths
            .OrderBy(lp => lp.Order)
            .IncludeStepsWithProgress()
            .ToList();

        // Формируем preview-модели
        return learningPathList
            .Select(learningPath => new LearningPathPreviewVm
            {
                Id = learningPath.LearningPathId,
                Title = learningPath.Title,
                Description = learningPath.Description,
                IsOpen = true,
                ImageUrl = learningPath.ImageUrl,
                Completion = _progressService
                    .GetPrecentegeFromStepProgress(learningPath.Steps)
            })
            .ToList();
    }

    /// <summary>
    /// Use-case: Начать путь обучения, инициировав первый шаг.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <returns>ID первого шага</returns>
    public StepNavigationVm StartLearningPath(int learningPathId)
    {
        // Загружаем агрегат LearningPath с шагами
        var learningPath = _ctx.LearningPaths.GetFullAggregateById(learningPathId);
        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь не найден");
        }

        // Находим первый шаг в пути
        var nextStep = _navigationService.FindFirstInPath(learningPath);
        if (nextStep == null)
        {
            throw new InvalidOperationException("Первый шаг не найден");
        }

        // Инициируем прогресс первого шага, если он ещё не начат
        _progressService.StartStepIfNeededV2(nextStep);
        _ctx.SaveChanges();

        // Возвращаем ID первого шага
        return new StepNavigationVm
        {
            Id = nextStep.StepId,
            LearningPathId = nextStep.LearningPathId
        };
    }

    /// <summary>
    /// Use-case: Завершить текущий шаг и вернуть ID следующего шага.
    /// </summary>
    /// <param name="learningPathId">ID пути</param>
    /// <param name="currentStepId">ID текущего шага</param>
    /// <returns>ID следующего шага (или null, если путь завершён)</returns>
    public StepNavigationVm? MarkStepAsCompletedAndProceed(int learningPathId, int currentStepId)
    {
        var learningPath = _ctx.LearningPaths.GetFullAggregateById(learningPathId);
        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь не найден");
        }

        var currentStep = _navigationService.FindStepInPathById(learningPath, currentStepId);
        if (currentStep == null)
        {
            throw new InvalidOperationException("Шаг не найден");
        }

        // Отмечаем шаг завершённым (веса прогресса жёстко заданы — 1f)
        _progressService.MarkStepAsCompletedV2(currentStep);

        var nextStep = _navigationService.FindNextStepInPath(learningPath, currentStep);
        if (nextStep == null)
        {
            _ctx.SaveChanges();
            return null;
        }

        _progressService.StartStepIfNeededV2(nextStep);
        _ctx.SaveChanges();

        return new StepNavigationVm
        {
            Id = nextStep.StepId,
            LearningPathId = nextStep.LearningPathId
        };
    }

    /// <summary>
    /// Use-case: Найти первый незавершённый шаг, чтобы продолжить обучение.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <returns>ID следующего шага или null</returns>
    public StepNavigationVm? GetStepToContinue(int learningPathId)
    {
        var learningPath = _ctx.LearningPaths.GetFullAggregateById(learningPathId);
        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь не найден");
        }

        var nextStep = _navigationService.FindContinueStepInPath(learningPath);
        if (nextStep == null)
        {
            return null;
        }

        return new StepNavigationVm
        {
            Id = nextStep.StepId,
            LearningPathId = nextStep.LearningPathId
        };
    }

    /// <summary>
    /// Use-case: Получить навигационную модель для конкретного шага.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <param name="stepId">ID шага</param>
    /// <returns>StepNavigationVm для шага</returns>
    public StepNavigationVm OpenStep(int learningPathId, int stepId)
    {
        // Загружаем путь обучения
        var learningPath = _ctx.LearningPaths.GetFullAggregateById(learningPathId);
        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь обучения не найден");
        }

        // Находим шаг в пути
        var step = _navigationService.FindStepInPathById(learningPath, stepId);
        if (step == null)
        {
            throw new InvalidOperationException("Шаг не найден");
        }

        // Получаем навигационную модель через NavigationService
        return _navigationService.GetStepNavigationVm(learningPath, step);
    }
}
