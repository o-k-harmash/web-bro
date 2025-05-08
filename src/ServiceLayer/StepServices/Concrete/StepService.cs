using DataLayer;
using WebBro.DataLayer.EfClasses;

public class StepService : IStepService
{
    private readonly AppDbContext _ctx;
    private readonly IProgressService _progressService;
    private readonly INavigationService _navigationService;
    private readonly IMarkdownService _markdownService;

    public StepService(
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
    /// Use-case: Получить подробную информацию о шаге.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <param name="stepId">ID шага</param>
    /// <returns>ViewModel с данными шага и ссылкой на следующий</returns>
    public StepDetailsViewModel GetStepDetails(int learningPathId, int stepId)
    {
        // Загружаем путь с шагами и прогрессами
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(s => s.LearningPathId == learningPathId);

        // Извлекаем нужный шаг
        var currentStep = _navigationService
            .FindStepInPathById(learningPath, stepId);

        // Определяем следующий шаг
        var nextStep = _navigationService
            .FindNextStepInPath(learningPath, currentStep);

        // Стартуем прогресс по текущему шагу, если он ещё не начат
        _progressService.StartStepIfNeeded(currentStep);
        _ctx.SaveChanges();

        return new StepDetailsViewModel
        {
            Id = currentStep.StepId,
            LearningPathId = currentStep.LearningPathId,
            Title = currentStep.Title,

            // TODO: Подгрузка markdown пока заглушена на README.md
            MarkdownContent = _markdownService.GetMarkdownContent("README.md"),

            // Передаём информацию о следующем шаге (если есть)
            NextStep = nextStep != null
                ? new PreviewViewModel
                {
                    Id = nextStep.StepId,
                    ImageUrl = nextStep.ImageUrl,
                    Title = nextStep.Title,
                    Description = nextStep.Description
                }
                : null
        };
    }

    /// <summary>
    /// Use-case: Найти первый незавершённый шаг, чтобы продолжить обучение.
    /// </summary>
    /// <param name="learningPathId">ID пути обучения</param>
    /// <returns>ID следующего шага или null</returns>
    public int? GetNextUnfinishedStep(int learningPathId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(s => s.LearningPathId == learningPathId);

        var continueStep = _navigationService
            .FindContinueStepInPath(learningPath);

        return continueStep?.StepId;
    }

    /// <summary>
    /// Use-case: Завершить текущий шаг и вернуть ID следующего шага.
    /// </summary>
    /// <param name="learningPathId">ID пути</param>
    /// <param name="currentStepId">ID текущего шага</param>
    /// <returns>ID следующего шага (или null, если путь завершён)</returns>
    public int? CompleteStep(int learningPathId, int currentStepId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(lp => lp.LearningPathId == learningPathId);

        var currentStep = _navigationService
            .FindStepInPathById(learningPath, currentStepId);

        var nextStep = _navigationService
            .FindNextStepInPath(learningPath, currentStep);

        // Отмечаем шаг завершённым (веса прогресса жёстко заданы — 1f)
        _progressService.AddCompleteForStepProgress(currentStep, StepCompletion.Completed);
        _ctx.SaveChanges();

        return nextStep?.StepId;
    }
}
