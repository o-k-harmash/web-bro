using Microsoft.EntityFrameworkCore;
using WebBro.DataLayer.EfClasses;

public static class LearningPathQueries
{
    /// <summary>
    /// Загружает путь обучения с шагами и прогрессом.
    /// </summary>
    public static IQueryable<LearningPath> IncludeStepsWithProgress(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .Include(lp => lp.Steps)
                .ThenInclude(sp => sp.StepProgress);
    }

    /// <summary>
    /// Загружает путь обучения с шагами и статьями.
    /// </summary>
    public static IQueryable<LearningPath> IncludeStepsWithArticles(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .Include(lp => lp.Steps)
                .ThenInclude(sp => sp.Article);
    }

    public static IQueryable<LearningPath> IncludeStepsWithChallenges(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .Include(lp => lp.Steps)
                .ThenInclude(sp => sp.Challenge);
    }

    /// <summary>
    /// Загружает путь обучения с шагами, прогрессом и статьями.
    /// </summary>
    public static IQueryable<LearningPath> IncludeStepsWithProgressAndArticles(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .IncludeStepsWithProgress()
            .IncludeStepsWithArticles();
    }

    /// <summary>
    /// Загружает путь обучения с шагами, прогрессом и челленджами.
    /// </summary>
    public static IQueryable<LearningPath> IncludeStepsWithProgressAndChallenges(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .IncludeStepsWithProgress()
            .Include(lp => lp.Steps)
                .ThenInclude(sp => sp.Challenge);
    }

    /// <summary>
    /// Получает путь обучения по ID с шагами, прогрессом и статьями.
    /// </summary>
    public static LearningPath? GetFullAggregateById(this IQueryable<LearningPath> learningPath, int learningPathId)
    {
        return learningPath
            .IncludeStepsWithProgress()
            .IncludeStepsWithArticles()
            .IncludeStepsWithChallenges()
            .SingleOrDefault(lp => lp.LearningPathId == learningPathId);
    }
}