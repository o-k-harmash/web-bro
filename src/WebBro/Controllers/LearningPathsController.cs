using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Webbro.V2;

[Route("learning-paths")]
public class LearningPathsController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILearningPathService _learningPathService;
    private readonly IStepService _stepService;
    private readonly ILogger<LearningPathsController> _logger;

    public LearningPathsController(
        IStepService stepService,
        ILogger<LearningPathsController> logger,
        IWebHostEnvironment hostEnvironment,
        ILearningPathService learningPathService)
    {
        _stepService = stepService;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _learningPathService = learningPathService;
    }

    // Use-case 0: Показ списка всех путей обучения (дашборд)
    // GET /learning-paths
    [HttpGet]
    public IActionResult Index()
    {
        var model = _learningPathService.GetLearningPathsPreview();
        return View(model);
    }

    // Use-case 1: Продолжить обучение — редирект на следующий незавершённый шаг
    // GET /learning-paths/{learningPathId}/continue
    [HttpGet("{learningPathId}/continue")]
    public IActionResult Continue(int learningPathId)
    {
        var stepId = _stepService.GetNextUnfinishedStep(learningPathId);

        if (stepId == null)
        {
            // Если все шаги завершены — возвращаемся на список
            return RedirectToAction("Index");
        }

        // Редирект на следующий шаг
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = stepId
        });
    }

    // Use-case 2: Открыть детальную страницу шага
    // GET /learning-paths/{learningPathId}/steps/{stepId}
    [HttpGet("{learningPathId}/steps/{stepId}")]
    public IActionResult OpenStep(int learningPathId, int stepId)
    {
        var vm = _stepService.GetStepDetails(learningPathId, stepId);
        return View("StepDetails", vm);
    }

    // Use-case 3: Завершить текущий шаг и перейти к следующему
    // GET /learning-paths/{learningPathId}/steps/{stepId}/complete
    [HttpGet("{learningPathId}/steps/{stepId}/complete")]
    public IActionResult CompleteStep(int learningPathId, int stepId)
    {
        var nextId = _stepService.CompleteStep(learningPathId, stepId);

        if (nextId == null)
        {
            // Если шагов больше нет — редирект на завершение пути
            return RedirectToAction("FinishPath", new { learningPathId });
        }

        // Иначе редирект на следующий шаг
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextId
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
