using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public IActionResult Index()
    {
        var model = _learningPathService
            .GetLearningPathsPreview();

        return View(model);
    }

    // 1. Продолжить путь
    [HttpGet("{learningPathId}/continue")]
    public IActionResult Continue(int learningPathId)
    {
        var stepId = _learningPathService
            .GetNextUnfinishedStep(learningPathId);

        if (stepId == null)
        {
            return RedirectToAction("Index");
        }

        return RedirectToAction(
            "OpenStep",
            new
            {
                learningPathId,
                stepId = stepId
            });
    }

    // 2. Открыть шаг
    [HttpGet("{learningPathId}/steps/{stepId}")]
    public IActionResult OpenStep(int learningPathId, int stepId)
    {
        var vm = _learningPathService
            .GetStepDetails(learningPathId, stepId);

        return View("StepDetails", vm);
    }

    // 3. Завершить шаг и перейти к следующему
    [HttpGet("{learningPathId}/steps/{stepId}/complete")]
    public IActionResult CompleteStep(int learningPathId, int stepId)
    {
        var nextId = _learningPathService.CompleteStep(learningPathId, stepId);

        if (nextId == null)
        {
            return RedirectToAction(
               "FinishPath",
               new { learningPathId });
        }

        return RedirectToAction(
            "OpenStep",
            new
            {
                learningPathId,
                stepId = nextId
            });
    }

    // (Опционально) Завершение пути
    [HttpGet("{learningPathId}/finish")]
    public IActionResult FinishPath(int learningPathId)
    {
        return RedirectToAction("Index");
    }

    [HttpGet("{learningPathId}/steps")]
    public IActionResult Details(int learningPathId)
    {
        var model = _learningPathService
          .GetLearningPathDetails(learningPathId);

        return View(model);
    }
}
