using Microsoft.AspNetCore.Mvc;
using WebBro.Services.Challenges;

namespace WebBro.Controllers;

[Route("learning-paths/{learningPathId}/steps/{stepId}/challenge")]
public class ChallengesController : Controller
{
    private readonly ILogger<ChallengesController> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IChallengeService _сhallengeService;

    public ChallengesController(
        ILogger<ChallengesController> logger,
        IWebHostEnvironment hostEnvironment,
        IChallengeService сhallengeService)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _сhallengeService = сhallengeService;
    }

    [HttpGet("start")]
    public IActionResult Start(int learningPathId, int stepId)
    {
        var challengeVm = _сhallengeService.PrepareChallengeStep(learningPathId, stepId);
        return View(challengeVm);
    }
}
