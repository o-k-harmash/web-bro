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

    // Use-case 0: Display a list of all learning paths (dashboard)
    // GET /learning-paths
    [HttpGet]
    public IActionResult Cource()
    {
        var model = _learningPathService.GetLearningPathsPreview();
        return View(model);
    }

    // Use-case 1: Start a learning path
    // GET /learning-paths/{learningPathId}/start
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

    // Use-case 2: Continue learning — redirect to the next unfinished step
    // GET /learning-paths/{learningPathId}/continue
    [HttpGet("{learningPathId}/continue")]
    public IActionResult Continue(int learningPathId)
    {
        //поправить поиск шагов проблема не видит что шаг не завершен по стейджам
        var nextNav = _learningPathService.GetStepToContinue(learningPathId);

        if (nextNav == null)
        {
            // If all steps are completed — redirect to the list
            return RedirectToAction("Cource");
        }

        // Redirect to the next step
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 3: Open a specific step in the learning path
    // GET /learning-paths/{learningPathId}/steps/{stepId}
    [HttpGet("{learningPathId}/steps/{stepId}")]
    public IActionResult OpenStep(int learningPathId, int stepId)
    {
        var nextNav = _learningPathService.OpenStep(learningPathId, stepId);

        // Return a view with the navigation model
        return RedirectToAction(nextNav.Stage, nextNav.Type, new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 4: Complete the current step and proceed to the next one
    // GET /learning-paths/{learningPathId}/steps/{stepId}/complete
    [HttpGet("{learningPathId}/steps/{stepId}/complete")]
    public IActionResult CompleteStep(int learningPathId, int stepId)
    {
        var nextNav = _learningPathService.MarkStepAsCompletedAndProceed(learningPathId, stepId);

        if (nextNav == null)
        {
            // If there are no more steps — redirect to finish the path
            return RedirectToAction("FinishPath", new { learningPathId });
        }

        // Redirect to the next step
        return RedirectToAction("OpenStep", new
        {
            learningPathId,
            stepId = nextNav.Id
        });
    }

    // Use-case 5: Finish the learning path — optional thank you or congratulations screen
    // GET /learning-paths/{learningPathId}/finish
    [HttpGet("{learningPathId}/finish")]
    public IActionResult FinishPath(int learningPathId)
    {
        // For now, just redirect to the list
        return RedirectToAction("Cource");
    }

    // Use-case 6: View details of the entire learning path with progress by steps
    // GET /learning-paths/{learningPathId}/steps
    [HttpGet("{learningPathId}/steps")]
    public IActionResult Details(int learningPathId)
    {
        var model = _learningPathService.GetLearningPathDetails(learningPathId);
        return View(model);
    }
}
