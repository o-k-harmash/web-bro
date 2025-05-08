using WebBro.DataLayer.EfClasses;

public interface ILearningPathService
{
    List<PreviewViewModel> GetLearningPathsPreview();
    LearningPathDetailsViewModel GetLearningPathDetails(int learningPathId);
}