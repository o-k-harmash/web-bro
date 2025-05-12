using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Webbro.V2;

[Route("learning-paths")]
public class LearningPathsController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILearningPathService _learningPathService;
    private readonly ILogger<LearningPathsController> _logger;

    public LearningPathsController(
        ILogger<LearningPathsController> logger,
        IWebHostEnvironment hostEnvironment,
        ILearningPathService learningPathService)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _learningPathService = learningPathService;
    }

    // Use-case 0: Показ списка всех путей обучения (дашборд)
    // GET /learning-paths
    [HttpGet]
    public IActionResult Cource()
    {
        var model = _learningPathService.GetLearningPathsPreview();
        return View(model);
    }

    [HttpGet("{learningPathId}/start")]
    public IActionResult Start(int learningPathId)
    {
        var nextNav = _learningPathService.StartLearningPath(learningPathId);

        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 1: Продолжить обучение — редирект на следующий незавершённый шаг
    // GET /learning-paths/{learningPathId}/continue
    [HttpGet("{learningPathId}/continue")]
    public IActionResult Continue(int learningPathId)
    {
        var nextNav = _learningPathService.GetStepToContinue(learningPathId);

        if (nextNav == null)
        {
            // Если все шаги завершены — возвращаемся на список
            return RedirectToAction("Index");
        }

        // Редирект на следующий шаг
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    [HttpGet("{learningPathId}/steps/{stepId}")]
    public IActionResult OpenStep(int learningPathId, int stepId)
    {
        var nextNav = _learningPathService.OpenStep(learningPathId, stepId);

        // Возвращаем вью с навигационной моделью
        return RedirectToAction(nextNav.Stage, nextNav.Type, new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 3: Завершить текущий шаг и перейти к следующему
    // GET /learning-paths/{learningPathId}/steps/{stepId}/complete
    [HttpGet("{learningPathId}/steps/{stepId}/complete")]
    public IActionResult CompleteStep(int learningPathId, int stepId)
    {
        var nextNav = _learningPathService.MarkStepAsCompletedAndProceed(learningPathId, stepId);

        if (nextNav == null)
        {
            // Если шагов больше нет — редирект на завершение пути
            return RedirectToAction("FinishPath", new { learningPathId });
        }

        // Редирект на следующий шаг
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 4 (опционально): Завершение пути — возможно экран благодарности или поздравление
    // GET /learning-paths/{learningPathId}/finish
    [HttpGet("{learningPathId}/finish")]
    public IActionResult FinishPath(int learningPathId)
    {
        // Пока просто редирект на список
        return RedirectToAction("Index");
    }

    // Use-case 5: Детали всего пути обучения с прогрессом по шагам
    // GET /learning-paths/{learningPathId}/steps
    [HttpGet("{learningPathId}/steps")]
    public IActionResult Details(int learningPathId)
    {
        var model = _learningPathService.GetLearningPathDetails(learningPathId);
        return View(model);
    }
}
