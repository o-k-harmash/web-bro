using DataLayer;

public class LearningPathService : ILearningPathService
{
    private readonly AppDbContext _ctx;
    private readonly IProgressService _progressService;
    private readonly INavigationService _navigationService;
    private readonly IMarkdownService _markdownService;

    public LearningPathService(
        AppDbContext ctx,
        IProgressService progressService,
        INavigationService navigationService,
        IMarkdownService markdownService)
    {
        _ctx = ctx;
        _progressService = progressService;
        _navigationService = navigationService;
        _markdownService = markdownService;
    }

    /// <summary>
    /// Use-case: Получить детальную модель пути обучения для UI.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <returns>ViewModel с шагами, прогрессом и описанием</returns>
    public LearningPathDetailsViewModel GetLearningPathDetails(int learningPathId)
    {
        // Загружаем агрегат LearningPath с шагами и прогрессом
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(lp => lp.LearningPathId == learningPathId);

        // Находим первый шаг и инициируем его прогресс при первом посещении
        var firstStep = _navigationService.FindFirstInPath(learningPath);
        _progressService.StartStepIfNeeded(firstStep);

        _ctx.SaveChanges();

        // Формируем модель для вьюхи
        return new LearningPathDetailsViewModel
        {
            Id = learningPath.LearningPathId,
            Title = learningPath.Title,
            Description = learningPath.Description,
            ImageUrl = learningPath.ImageUrl,

            // Общий процент завершения по всем шагам
            Completion = _progressService.GetPrecentegeFromStepProgress(learningPath.Steps),

            // Отображение всех шагов с индивидуальным прогрессом
            Steps = learningPath.Steps
                .OrderBy(sp => sp.Order)
                .Select(sp => new PreviewViewModel
                {
                    Id = sp.StepId,
                    Title = sp.Title,
                    Description = sp.Description,
                    ImageUrl = sp.ImageUrl,
                    Completion = _progressService.GetPrecentegeFromStepProgress(sp)
                })
                .ToList()
        };
    }

    /// <summary>
    /// Use-case: Получить список всех путей обучения с кратким описанием и прогрессом.
    /// </summary>
    /// <returns>Список Preview моделей для вывода на главной</returns>
    public List<PreviewViewModel> GetLearningPathsPreview()
    {
        // Загружаем все пути обучения с шагами и прогрессом
        var learningPathList = _ctx.LearningPaths
            .OrderBy(lp => lp.Order)
            .AggregateWithStepsAndProgresses()
            .ToList();

        // Формируем preview-модели
        return learningPathList
            .Select(learningPath => new PreviewViewModel
            {
                Id = learningPath.LearningPathId,
                Title = learningPath.Title,
                Description = learningPath.Description,
                ImageUrl = learningPath.ImageUrl,
                Completion = _progressService.GetPrecentegeFromStepProgress(learningPath.Steps)
            })
            .ToList();
    }
}
