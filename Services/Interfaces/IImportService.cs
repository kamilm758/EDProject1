using EDProject1.Models;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace EDProject1.Services.Interfaces
{
    public interface IImportService
    {
        DataTable ImportTextData(UploadFileModel model);
        DataTable ImportExcelData(IFormFile file);
        DataTable ImportData(UploadFileModel model);
    }
}
