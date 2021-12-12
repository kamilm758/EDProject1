using EDProject1.Models;

namespace EDProject1.Services.Interfaces
{
    public interface IClassificationService
    {
        string ClassifyNewObject(ClassificationModeVm model);
    }
}
