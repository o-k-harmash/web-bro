using DataLayer;
using WebBro.DataLayer.EfClasses;

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

    public LearningPathDetailsViewModel GetLearningPathDetails(int learningPathId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(lp => lp.LearningPathId == learningPathId);

        var firstStep = _navigationService
            .FindFirstInPath(learningPath);

        _progressService.StartStepIfNeeded(firstStep);

        _ctx.SaveChanges();

        return new LearningPathDetailsViewModel
        {
            Id = learningPath.LearningPathId,
            Title = learningPath.Title,
            Description = learningPath.Description,
            ImageUrl = learningPath.ImageUrl,
            Completion = _progressService
                .GetPrecentegeFromStepProgress(learningPath.Steps),
            Steps = learningPath.Steps
                .OrderBy(sp => sp.Order)
                .Select(sp => new PreviewViewModel
                {
                    Id = sp.StepId,
                    Title = sp.Title,
                    Description = sp.Description,
                    ImageUrl = sp.ImageUrl,
                    Completion = _progressService
                        .GetPrecentegeFromStepProgress(sp)
                })
                .ToList()
        };
    }

    public List<PreviewViewModel> GetLearningPathsPreview()
    {
        var learningPathList = _ctx.LearningPaths
            .OrderBy(lp => lp.Order)
            .AggregateWithStepsAndProgresses()
            .ToList();

        return learningPathList
            .Select(learningPath => new PreviewViewModel
            {
                Id = learningPath.LearningPathId,
                Title = learningPath.Title,
                Description = learningPath.Description,
                ImageUrl = learningPath.ImageUrl,
                Completion = _progressService
                    .GetPrecentegeFromStepProgress(learningPath.Steps)
            })
            .ToList();
    }

    public StepDetailsViewModel GetStepDetails(int learningPathId, int stepId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(s => s.LearningPathId == learningPathId);

        var currentStep = _navigationService
            .FindStepInPathById(learningPath, stepId);

        var nextStep = _navigationService
            .FindNextStepInPath(learningPath, currentStep);

        _progressService.StartStepIfNeeded(currentStep);

        _ctx.SaveChanges();

        return new StepDetailsViewModel
        {
            Id = currentStep.StepId,
            LearningPathId = currentStep.LearningPathId,
            Title = currentStep.Title,
            MarkdownContent = _markdownService
                .GetMarkdownContent("README.md"),
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

    public int? GetNextUnfinishedStep(int learningPathId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(s => s.LearningPathId == learningPathId);

        var continueStep = _navigationService
            .FindContinueStepInPath(learningPath);

        return continueStep?.StepId;
    }

    public int? CompleteStep(int learningPathId, int currentStepId)
    {
        var learningPath = _ctx.LearningPaths
            .AggregateWithStepsAndProgresses()
            .Single(lp => lp.LearningPathId == learningPathId);

        var currentStep = _navigationService
            .FindStepInPathById(learningPath, currentStepId);

        var nextStep = _navigationService
            .FindNextStepInPath(learningPath, currentStep);

        _progressService.AddCompleteForStepProgress(currentStep, 1f);

        _ctx.SaveChanges();

        return nextStep?.StepId;
    }
}