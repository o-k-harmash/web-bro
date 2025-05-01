
using Microsoft.AspNetCore.Mvc;
using Markdig;

namespace Webbro.V2;

[Route("learning-paths")]
public class LearningPathsController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;

    // Имитируем "базу данных"
    private static readonly List<LearningPathViewModel> _learningPaths = new()
    {
        new LearningPathViewModel
        {
            ContinueLink = "http://localhost:5080/learning-paths/1/steps/2",
            Link = "http://localhost:5080/learning-paths/1/steps",
            Id = 1,
            CompletedPercentage = 50,
            Title = "Getting started on Frontend Mentor",
            Description = "These projects will help you find your feet on the Frontend Mentor platform and give you experience working with designs and building small projects. They're all HTML & CSS-only challenges, so they'll help you pick up the basics.",
            Tag = "Backend",
            ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/samkit9vyygeuxqi6f4q.jpg",
            Steps = new List<StepViewModel>
            {
                new StepViewModel
                {
                    Id = 1,
                    LearningPathId = 1,
                    Title = "Introduction to Frontend Mentor",
                    Description = "In this article, we help you get up to speed with the Frontend Mentor platform and what we offer. After reading, you'll be aware of why we exist and will understand our platform's key areas.",
                    Tag = "Setup",
                    ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                    Order = 1,
                    CompletedPercentage = 100,
                    Link = "http://localhost:5080/learning-paths/1/steps/1"
                },
                new StepViewModel
                {
                    Id = 2,
                    LearningPathId = 1,
                    Title = "Setting up your development environment",
                    Description = "This article will take you through setting up your computer with all the tools you need to complete challenges on Frontend Mentor successfully. We also introduce you to Git version control.",
                    Tag = "Concept",
                    ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/djdmx8w0yjieccg3t3qw.png",
                    Order = 2,
                    CompletedPercentage = 0,
                    Link = "http://localhost:5080/learning-paths/1/steps/2/challenge/start"
                }
            }
        }
    };

    private readonly ILogger<LearningPathsController> _logger;

    public LearningPathsController(ILogger<LearningPathsController> logger, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = _learningPaths
           .Select(lp => new LearningPathViewModel
           {
               Id = lp.Id,
               Title = lp.Title,
               Description = lp.Description,
               Tag = lp.Tag,
               ImageUrl = lp.ImageUrl,
               Steps = lp.Steps,
               ContinueLink = lp.ContinueLink,
               CompletedPercentage = lp.CompletedPercentage,
               Link = lp.Link
           })
           .ToList();
        return View(model);
    }

    [HttpGet("{pathId}/steps/{stepId}")]
    public IActionResult StepDetails(int pathId, int stepId)
    {
        var learningPath = _learningPaths.FirstOrDefault(lp => lp.Id == pathId);
        if (learningPath == null)
            return NotFound();

        var currentStep = learningPath.Steps.FirstOrDefault(s => s.Id == stepId);
        if (currentStep == null)
            return NotFound();

        //if not added
        // _learningPathsProgress.Add(new StepProgressViewModel
        // {
        //     LearningPathId = pathId,
        //     StepId = stepId,
        //     CompletedAt = null
        // });

        var nextStep = learningPath.Steps
            .Where(s => s.Order > currentStep.Order)
            .OrderBy(s => s.Order)
            .FirstOrDefault();

        var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "README.md");
        var markdownContent = Markdown.ToHtml(System.IO.File.ReadAllText(filePath));

        var model = new StepDetailsViewModel
        {
            Id = currentStep.Id,
            Title = currentStep.Title,
            LearningPathId = currentStep.LearningPathId,
            MarkdownContent = markdownContent, // Заглушка контента
            NextStep = nextStep == null ? null : new StepViewModel
            {
                Id = nextStep.Id,
                Title = nextStep.Title,
                Tag = nextStep.Tag,
                Description = nextStep.Description,
                ImageUrl = nextStep.ImageUrl,
                Link = nextStep.Link
            }
        };

        return View(model);
    }

    [HttpGet("{pathId}/steps")]
    public IActionResult Details(int pathId)
    {
        var learningPath = _learningPaths.FirstOrDefault(lp => lp.Id == pathId);
        if (learningPath == null)
            return NotFound();

        //if not added
        // _learningPathsProgress.Add(new StepProgressViewModel
        // {
        //     LearningPathId = pathId,
        //     StepId = firstStepId,
        //     CompletedAt = null
        // });

        return View(model: learningPath);
    }
}
