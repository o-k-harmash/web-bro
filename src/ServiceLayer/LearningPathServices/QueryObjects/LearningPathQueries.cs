using Microsoft.EntityFrameworkCore;
using WebBro.DataLayer.EfClasses;

public static class LearningPathQueries
{
    public static IQueryable<LearningPath> AggregateWithStepsAndProgresses(this IQueryable<LearningPath> learningPath)
    {
        return learningPath
            .Include(lp => lp.Steps)
            .ThenInclude(sp => sp.StepProgress);
    }
}