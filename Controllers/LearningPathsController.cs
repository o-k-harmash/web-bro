using Microsoft.AspNetCore.Mvc;
using WebBro.ViewModels;
using Markdig;

namespace WebBro.Controllers;

[Route("learning-paths")]
public class LearningPathsController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;

    // Хранилище прогресса для LearningPath c Id = 1
    // Хранилище прогресса для LearningPath c Id = 1
    private static readonly List<StepProgressViewModel> _learningPathsProgress = new()
{
    new StepProgressViewModel
    {
        LearningPathId = 1,
        StepId = 1,
        CompletedAt = DateTime.UtcNow.AddDays(-1) // прошел вчера
    },
    // второй шаг еще не пройден

    new StepProgressViewModel
    {
        LearningPathId = 2,
        StepId = 3,
        CompletedAt = DateTime.UtcNow.AddDays(-2) // прошел позавчера
    },

    new StepProgressViewModel
    {
        LearningPathId = 3,
        StepId = 5,
        CompletedAt = DateTime.UtcNow.AddDays(-5) // давно пройден
    },
    new StepProgressViewModel
    {
        LearningPathId = 3,
        StepId = 6,
        CompletedAt = DateTime.UtcNow.AddDays(-3) // прошел недавно
    }
};



    // Имитируем "базу данных"
    private static readonly List<LearningPathDetailViewModel> _learningPaths = new()
{
    new LearningPathDetailViewModel
    {
        Id = 1,
        Title = "Getting started on Frontend Mentor",
        Description = "These projects will help you find your feet on the Frontend Mentor platform and give you experience working with designs and building small projects. They're all HTML & CSS-only challenges, so they'll help you pick up the basics.",
        Tag = "Backend",
        ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/samkit9vyygeuxqi6f4q.jpg",
        Steps = new List<StepPreviewViewModel>
        {
            new StepPreviewViewModel
            {
                Id = 1,
                LearningPathId = 1,
                Title = "Introduction to Frontend Mentor",
                Description = "In this article, we help you get up to speed with the Frontend Mentor platform and what we offer. After reading, you'll be aware of why we exist and will understand our platform's key areas.",
                Tag = "Setup",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 1
            },
            new StepPreviewViewModel
            {
                Id = 2,
                LearningPathId = 1,
                Title = "Setting up your development environment",
                Description = "This article will take you through setting up your computer with all the tools you need to complete challenges on Frontend Mentor successfully. We also introduce you to Git version control.",
                Tag = "Concept",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/djdmx8w0yjieccg3t3qw.png",
                Order = 2
            }
        }
    },
    new LearningPathDetailViewModel
    {
        Id = 2,
        Title = "Building responsive layouts",
        Description = "A crucial part of modern front-end development is making web pages look good on various device sizes. The challenges in this path are designed to help you get to grips with making layouts that work across all devices.",
        Tag = "Backend",
        ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/svxn9hckf62ieldx5lqz.jpg",
        Steps = new List<StepPreviewViewModel>
        {
            new StepPreviewViewModel
            {
                Id = 3,
                LearningPathId = 2,
                Title = "Responsive design fundamentals",
                Description = "It's essential to lay a strong foundation in responsive design so that your projects adapt to different devices. In this article, we'll outline the basics and link to some excellent resources.",
                Tag = "Setup",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 1
            },
            new StepPreviewViewModel
            {
                Id = 4,
                LearningPathId = 2,
                Title = "Responsive media and fluid typography",
                Description = "Ensuring your media is optimized, and your text is readable on all devices will significantly impact your visitors. In this article, we discuss the key considerations.",
                Tag = "Database",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 2
            }
        }
    },
    new LearningPathDetailViewModel
    {
        Id = 3,
        Title = "JavaScript fundamentals",
        Description = "Adding interactivity to our web pages is a key skill of the front-end developer. The challenges in this path are designed to guide you through handling common user interactions with JavaScript.",
        Tag = "Frontend",
        ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/vcu7qvitarxosrblkpld.png",
        Steps = new List<StepPreviewViewModel>
        {
            new StepPreviewViewModel
            {
                Id = 5,
                LearningPathId = 3,
                Title = "Welcome to the JS fundamentals path!",
                Description = "This article gives you an overview of the JS fundamentals path, why you should learn JavaScript, and how you can get help along the way.",
                Tag = "HTML",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/b1kthsjafebmzgmny8hz.jpg",
                Order = 1
            },
            new StepPreviewViewModel
            {
                Id = 6,
                LearningPathId = 3,
                Title = "Setting up your project to use JavaScript",
                Description = "This article provides you with a simple and robust local setup for JavaScript projects.",
                Tag = "CSS",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/vxtsvktotunfghb6xwqv.jpg",
                Order = 2
            },
            new StepPreviewViewModel
            {
                Id = 7,
                LearningPathId = 3,
                Title = "The Document Object Model",
                Description = "As the interface between HTML and JavaScript, the Document Object Model is a fundamental concept to working with JavaScript. This article gives you an overview of the DOM and how to interact with it.",
                Tag = "JavaScript",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/f2hi8vmenfkiuny7aiua.jpg",
                Order = 3
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
           .Select(lp => new LearningPathPreviewViewModel
           {
               Id = lp.Id,
               Title = lp.Title,
               Description = lp.Description,
               Tag = lp.Tag,
               ImageUrl = lp.ImageUrl,
               Steps = lp.Steps,
               StepsProgress = _learningPathsProgress.Where(p => lp.Id == p.LearningPathId).ToList()
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

        var nextStep = learningPath.Steps
            .Where(s => s.Order > currentStep.Order)
            .OrderBy(s => s.Order)
            .FirstOrDefault();

        var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "README.md");
        var markdownContent = Markdown.ToHtml(System.IO.File.ReadAllText(filePath));

        var model = new StepDetailViewModel
        {
            Id = currentStep.Id,
            Title = currentStep.Title,
            LearningPathId = currentStep.LearningPathId,
            MarkdownContent = markdownContent, // Заглушка контента
            NextStep = nextStep == null ? null : new NextStepPreviewViewModel
            {
                Id = nextStep.Id,
                Title = nextStep.Title,
                Tag = nextStep.Tag,
                Description = nextStep.Description,
                ImageUrl = nextStep.ImageUrl
            },
            NavbarSteps = learningPath.Steps
                .OrderBy(s => s.Order)
                .Select(s => new NavbarStepLinkViewModel
                {
                    Id = s.Id,
                    Title = s.Title
                })
                .ToList()
        };

        return View(model);
    }

    [HttpGet("{pathId}/steps/")]
    public IActionResult Details(int pathId)
    {
        var learningPath = _learningPaths.FirstOrDefault(lp => lp.Id == pathId);
        if (learningPath == null)
            return NotFound();

        learningPath.StepsProgress = _learningPathsProgress.Where(lp => lp.LearningPathId == pathId).ToList();
        learningPath.Steps.Select(s => s.IsCompleted = _learningPathsProgress.FirstOrDefault(p => p.StepId == s.Id) != null ? true : false).ToList();

        var lastStep = learningPath.StepsProgress.OrderBy(sp => sp.StepId).FirstOrDefault(); /*заглушка в реальности подтянуть степ детали и сортировать по ордеру*/
        /*Добавить логику обработки пустого прогресса*/
        var activeStep = learningPath.Steps.Where(sp => sp.Id > lastStep.StepId).FirstOrDefault();

        learningPath.ContinueStepId = activeStep == null ? lastStep.StepId : activeStep.Id;

        return View(model: learningPath);
    }
}
