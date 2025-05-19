using DataLayer;

public class LearningPathNavigationService : ILearningPathNavigationService
{
    private readonly AppDbContext _ctx;
    private readonly IProgressService _progressService;
    private readonly INavigationService _navigationService;

    public LearningPathNavigationService(AppDbContext ctx, IProgressService progressService, INavigationService navigationService)
    {
        _ctx = ctx;
        _progressService = progressService;
        _navigationService = navigationService;
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

    public List<NavItemVm> GetLearningPathNavigation(int learningPathId, int stepId, string stageKey)
    {
        var learningPath = _ctx.LearningPaths.GetFullAggregateById(learningPathId);
        if (learningPath == null)
        {
            throw new InvalidOperationException("Путь обучения не найден");
        }

        // Находим шаг в пути
        var step = _navigationService.FindStepInPathById(learningPath, stepId);

        var stage = _navigationService.FindStageByKeyV2(step, stageKey);

        var navItemVms = new List<NavItemVm> { };

        foreach (var sp in learningPath.Steps)
        {
            var childrens = sp.StageList
                .Select(st => new NavItemVm
                {
                    StepId = sp.StepId,
                    Title = st.StageKey,
                    LearningPathId = learningPathId,

                    IsOpen = _progressService
                    .FindStageProgress(sp, st) != null,

                    IsCompleted = _progressService
                        .FindStageProgress(sp, st)?.Completion == StepCompletion.Completed,

                    IsCurrentPage = stage.StageKey == st.StageKey,
                    Type = sp.Type,
                    StageKey = st.StageKey
                })
                .ToList();

            var isSingle = childrens.Count == 1;

            var stepProgress = _progressService.FindStepProgress(sp);

            var navItemVm = new NavItemVm
            {
                StepId = sp.StepId,
                Title = sp.Title,
                LearningPathId = learningPathId,
                IsOpen = stepProgress != null,
                IsCompleted = stepProgress?.Completion == StepCompletion.Completed,
            };

            if (isSingle)
            {
                var one = childrens.First();
                navItemVm.Type = sp.Type;
                navItemVm.StageKey = one.StageKey;
            }
            else
            {
                navItemVm.Children = childrens;
            }

            navItemVms.Add(navItemVm);
        }

        return navItemVms;
    }
}
