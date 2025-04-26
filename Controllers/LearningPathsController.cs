using Microsoft.AspNetCore.Mvc;
using WebBro.Models;
using WebBro.ViewModel;

namespace WebBro.Controllers;

[Route("learning-paths")]
public class LearningPathsController : Controller
{
    private readonly List<LearningPath> _learningPaths = new List<LearningPath> {
            new LearningPath
            {
                Id = "8a7e607d9b31ab036ba62de9ff276eb2a1ba5623e0b2d35bc683017f243bd5ed",
                Name = "Getting started on Frontend Mentor",
                Description = "These projects will help you find your feet on the Frontend Mentor platform and give you experience working with designs and building small projects. They're all HTML & CSS-only challenges, so they'll help you pick up the basics.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/samkit9vyygeuxqi6f4q.jpg",
                Tag = "newbie"
            },
             new LearningPath
            {
                Id = "e684ca0f5ad9ebf4c87fa0fb7bd28c8fc660f07ad986e67a12c22b90c43ac724",
                Name = "Building responsive layouts",
                Description = "A crucial part of modern front-end development is making web pages look good on various device sizes. The challenges in this path are designed to help you get to grips with making layouts that work across all devices.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/svxn9hckf62ieldx5lqz.jpg",
                Tag = "newbie"
            },
             new LearningPath
            {
                Id = "a3c3a009889d8e58e5b7ffe7433325e00bfbcd8c7684a1b493310392f2d0b83f",
                Name = "JavaScript fundamentals",
                Description = "Adding interactivity to our web pages is a key skill of the front-end developer. The challenges in this path are designed to guide you through handling common user interactions with JavaScript.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/vcu7qvitarxosrblkpld.png",
                Tag = "junior"
            }};

    private readonly List<Step> _Steps = new List<Step> {
            new Step
            {
                Id = "1",
                LearningPathId = "8a7e607d9b31ab036ba62de9ff276eb2a1ba5623e0b2d35bc683017f243bd5ed",
                Name = "Getting started on Frontend Mentor",
                Order = 0
            },
            new Step
            {
                Id = "2",
                LearningPathId = "8a7e607d9b31ab036ba62de9ff276eb2a1ba5623e0b2d35bc683017f243bd5ed",
                Name = "Getting started on Frontend Mentor 2",
                Order = 1
            },
             new Step
            {
                Id = "3",
                LearningPathId = "e684ca0f5ad9ebf4c87fa0fb7bd28c8fc660f07ad986e67a12c22b90c43ac724",
                Name = "Building responsive layouts",
                Order = 0
            },
                new Step
            {
                Id = "4",
                LearningPathId = "e684ca0f5ad9ebf4c87fa0fb7bd28c8fc660f07ad986e67a12c22b90c43ac724",
                Name = "Building responsive layouts 2",
                Order = 1
            },
             new Step
            {
                Id = "5",
                LearningPathId = "a3c3a009889d8e58e5b7ffe7433325e00bfbcd8c7684a1b493310392f2d0b83f",
                Name = "JavaScript fundamentals",
                Order = 0
            },
             new Step
            {
                Id = "6",
                LearningPathId = "a3c3a009889d8e58e5b7ffe7433325e00bfbcd8c7684a1b493310392f2d0b83f",
                Name = "JavaScript fundamentals2",
                Order = 1
            }};

    private readonly ILogger<LearningPathsController> _logger;

    public LearningPathsController(ILogger<LearningPathsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = GetLearningPathList();
        var viewModels = model.Select(p => new LearningPathViewModel { LearningPath = p, NextStep = GetStepList(p.Id).First(s => s.LearningPathId == p.Id) }).ToList();
        _logger.LogInformation($"{model.Count()}");
        return View(model: viewModels);
    }

    [HttpGet("{pathId}/steps/{stepId}")]
    public IActionResult Step(string pathId, string stepId)
    {
        var model = GetStepById(pathId, stepId);
        var viewModels = new StepViewModel { StepList = GetStepList(pathId), CurrentStep = model, NextStep = GetStepList(pathId).FirstOrDefault(c => c.Order > model.Order) };

        return View(model: viewModels);
    }

    private List<LearningPath> GetLearningPathList()
    {
        return _learningPaths;
    }

    private List<Step> GetStepList(string pathId)
    {
        return _Steps.FindAll(s => s.LearningPathId == pathId);
    }

    private Step GetStepById(string pathId, string stepId)
    {
        return _Steps.Single(s => s.LearningPathId == pathId && s.Id == stepId);
    }

    private string GetFirstStepInPath(string pathId)
    {
        return _learningPaths.Single(p => p.Id == pathId).Description;
    }
}
