using Microsoft.AspNetCore.Mvc;

namespace WebBro.Controllers;

[Route("learning-paths/{pathId}/steps/{stepId}/challenge")]
public class ChallengesController : Controller
{
    private readonly ILogger<ChallengesController> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ChallengesController(ILogger<ChallengesController> logger, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("start")]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet("submit")]
    public IActionResult Submit()
    {
        return View();
    }

    [HttpGet("improve")]
    public IActionResult Improve()
    {
        return View();
    }

    [HttpGet("review")]
    public IActionResult Review()
    {
        return View();
    }

    [HttpGet("complete")]
    public IActionResult Complete()
    {
        return View();
    }
}
