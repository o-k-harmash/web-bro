using WebBro.DataLayer.EfClasses;

namespace WebBro.Services.Challenges
{
    public interface IChallengeService
    {
        // Методы для работы с челленджами
        StepNavigationVm CompleteChallengeStage(int learningPathId, int stepId, string stage);
        ChallengeStartVm PrepareChallengeStep(int learningPathId, int stepId);
        ChallengeSubmitVm ValidateChallengeSubmission(int learningPathId, int stepId);
    }
}
