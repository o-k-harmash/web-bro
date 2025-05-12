using WebBro.DataLayer.EfClasses;

namespace WebBro.Services.Challenges
{
    public interface IChallengeService
    {
        // Методы для работы с челленджами
        ChallengeStartVm PrepareChallengeStep(int learningPathId, int stepId);
        ChallengeSubmitVm ValidateChallengeSubmission(int learningPathId, int stepId);
    }
}
