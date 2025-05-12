using Microsoft.AspNetCore.Mvc;
using WebBro.Services.Articles;

namespace WebBro.Controllers
{
    [Route("learning-paths/{learningPathId}/steps/{stepId}/article")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult Read(int learningPathId, int stepId)
        {
            var vm = _articleService.GetArticleForReading(learningPathId, stepId);
            return View(vm);
        }
    }
}
