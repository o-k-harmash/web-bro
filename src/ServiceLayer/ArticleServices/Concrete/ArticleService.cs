using DataLayer;
using WebBro.DataLayer.EfClasses;

namespace WebBro.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _ctx;
        private readonly IProgressService _progressService;
        private readonly INavigationService _navigationService;
        private readonly IMarkdownService _markdownService;
        private readonly ILearningPathNavigationService _learningPathNavigationService;

        public ArticleService(
            AppDbContext ctx,
            IProgressService progressService,
            INavigationService navigationService,
            IMarkdownService markdownService,
            ILearningPathNavigationService learningPathNavigationService)
        {
            _ctx = ctx;
            _progressService = progressService;
            _navigationService = navigationService;
            _markdownService = markdownService;
            _learningPathNavigationService = learningPathNavigationService;
        }

        /// <summary>
        /// Use-case: Получить подробную информацию о шаге.
        /// </summary>
        /// <param name="learningPathId">ID пути обучения</param>
        /// <param name="stepId">ID шага</param>
        /// <returns>ViewModel с данными шага и ссылкой на следующий</returns>
        public ArticleReadVm GetArticleForReading(int learningPathId, int stepId)
        {
            // Загружаем путь с шагами и прогрессами
            var learningPath = _ctx.LearningPaths
                .IncludeStepsWithArticles()
                .FirstOrDefault(lp => lp.LearningPathId == learningPathId);

            if (learningPath == null)
            {
                throw new InvalidOperationException("Путь не найден");
            }

            // Извлекаем нужный шаг
            var currentStep = _navigationService.FindStepInPathById(learningPath, stepId);

            var article = currentStep.Article;
            if (article == null)
            {
                throw new InvalidOperationException("Статья не найдена");
            }

            // Определяем следующий шаг
            var nextStep = _navigationService.FindNextStepInPath(learningPath, currentStep);

            // Стартуем прогресс по текущему шагу, если он ещё не начат
            // _progressService.StartStepIfNeeded(currentStep);
            // _ctx.SaveChanges();

            return new ArticleReadVm
            {
                Id = currentStep.StepId,
                LearningPathId = currentStep.LearningPathId,
                Title = currentStep.Title,

                // TODO: Подгрузка markdown пока заглушена на README.md
                MarkdownContent = _markdownService
                    .GetMarkdownContent(article.ArticleMarkdown),

                // Передаём информацию о следующем шаге (если есть)
                NextStep = nextStep != null
                    ? new LearningPathPreviewVm
                    {
                        Id = nextStep.StepId,
                        ImageUrl = nextStep.ImageUrl,
                        Title = nextStep.Title,
                        Description = nextStep.Description
                    }
                    : null,

                StepNavs = _learningPathNavigationService
                    .GetLearningPathNavigation(learningPathId, stepId, "read")
            };
        }
    }
}
