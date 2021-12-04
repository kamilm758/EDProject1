using EDProject1.Algorithm;
using EDProject1.Extensions;
using EDProject1.Models;
using EDProject1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EDProject1.Controllers
{
    public class ImportController : Controller
    {
        private readonly IImportService _importService;
        private readonly DataStructure _dataStructure;
        public ImportController(IImportService importService, DataStructure dataStructure)
        {
            _importService = importService;
            _dataStructure = dataStructure;

        }

        public IActionResult Index()
        {
            return View(new UploadFileModel());
        }

        [HttpPost]
        public IActionResult Index(UploadFileModel model)
        {
            if (model is not null)
            {
                DataTable dataTable = _importService.ImportData(model);
                _dataStructure.InitializeColumnTypes(dataTable.GetColumnNames());
                _dataStructure.ImportData(dataTable);
            }
            AlgorithmProcessor algorithmProcessor = new AlgorithmProcessor(_dataStructure);

            algorithmProcessor.Run();
            return RedirectToAction("Index", "Home");
        }
    }
}
