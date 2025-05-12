using WebBro.DataLayer.EfClasses;

namespace WebBro.Services.Articles
{
    public interface IArticleService
    {
        // Методы для работы со статьями
        ArticleReadVm GetArticleForReading(int learningPathId, int stepId);
    }
}
