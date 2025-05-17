using DataLayer;
using WebBro.DataLayer.EfClasses;

namespace WebBro.Services.Challenges
{
    public class ChallengeService : IChallengeService
    {
        private readonly AppDbContext _ctx;
        private readonly IProgressService _progressService;
        private readonly INavigationService _navigationService;
        private readonly IMarkdownService _markdownService;

        public ChallengeService(
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

        public ChallengeStartVm PrepareChallengeStep(int learningPathId, int stepId)
        {
            // 1. Загружаем путь с шагами и прогрессами
            var learningPath = _ctx.LearningPaths
                .IncludeStepsWithChallenges()
                .FirstOrDefault(lp => lp.LearningPathId == learningPathId);

            if (learningPath == null)
            {
                throw new InvalidOperationException("Путь не найден");
            }

            // 2. Извлекаем нужный шаг
            var currentStep = _navigationService.FindStepInPathById(learningPath, stepId);
            if (currentStep == null)
            {
                throw new InvalidOperationException("Шаг не найден");
            }

            // 3. Проверяем, что шаг является челленджем
            var challenge = currentStep.Challenge;
            if (challenge == null)
            {
                throw new InvalidOperationException("Шаг не является челленджем");
            }

            // 6. Мапим данные челленджа во ViewModel
            return new ChallengeStartVm
            {
                Id = currentStep.StepId,
                LearningPathId = currentStep.LearningPathId,
                DesktopPreviewImage = challenge.DesktopPreviewImage,
                MobilePreviewImage = challenge.MobilePreviewImage,

                // TODO: Подгрузка markdown пока заглушена на README.md
                BriefMarkdown = _markdownService
                    .GetMarkdownContent(challenge.BriefMarkdown),

                // TODO: Подгрузка markdown пока заглушена на README.md
                SuggestionMarkdown = _markdownService
                    .GetMarkdownContent(challenge.SuggestionMarkdown),

                SolutionBaseRepository = challenge.SolutionBaseRepository,

                Step = new LearningPathPreviewVm
                {
                    Id = currentStep.StepId,
                    Title = currentStep.Title,
                    Description = currentStep.Description,
                    ImageUrl = currentStep.ImageUrl,
                }
            };
        }

        public ChallengeSubmitVm ValidateChallengeSubmission(int learningPathId, int stepId)
        {
            throw new NotImplementedException();
        }

        public StepNavigationVm CompleteChallengeStage(int learningPathId, int stepId, string stageKey)
        {
            // 1. Загружаем путь с шагами и прогрессами
            var learningPath = _ctx.LearningPaths
                .IncludeStepsWithProgressAndChallenges()
                .FirstOrDefault(lp => lp.LearningPathId == learningPathId);

            if (learningPath == null)
            {
                throw new InvalidOperationException("Путь обучения не найден");
            }

            // 2. Извлекаем нужный шаг
            var currentStep = _navigationService.FindStepInPathById(learningPath, stepId);
            if (currentStep == null)
            {
                throw new InvalidOperationException("Шаг не найден");
            }

            // 3. Проверяем, что шаг является челленджем
            if (currentStep.Challenge == null)
            {
                throw new InvalidOperationException("Шаг не является челленджем");
            }

            var stage = _navigationService.FindStageByKeyV2(currentStep, stageKey);
            var nextStage = _navigationService.FindNextStageInStepV2(currentStep, stage);

            _progressService.MarkStageAsCompletedV2(currentStep, stage);
            _progressService.StartStageIfNeededV2(currentStep, nextStage);
            _ctx.SaveChanges();

            //завершить текущий стейдж +
            //и возможно обновить прогресс шага если текущий стейдж не был завешен +

            return new StepNavigationVm
            {
                Id = currentStep.StepId,
                LearningPathId = currentStep.LearningPathId,
                Stage = nextStage.StageKey
                //найти следующий стейдж +
            };
        }
    }
}
