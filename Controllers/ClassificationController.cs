using EDProject1.Enums;
using EDProject1.Models;
using EDProject1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EDProject1.Controllers
{
    public class ClassificationController : Controller
    {
        private readonly DataStructure _dataStructure;
        private readonly IClassificationService _classificationService;

        public ClassificationController(DataStructure dataStructure, IClassificationService classificationService)
        {
            _dataStructure = dataStructure;
            _classificationService = classificationService;
        }


       
        public IActionResult ClassificationMode(string klasaDecyzyjnaDropdown)
        {
            ClassificationModeVm model = new();
          
            List<string> columnNames = _dataStructure.GetColumnNames();
            Dictionary<string, ColumnType> columnTypes = new Dictionary<string, ColumnType>();
            Dictionary<string, string> rowValues = new Dictionary<string, string>();


            foreach (var columnName in columnNames.Where(x => x != klasaDecyzyjnaDropdown))
            {
                columnTypes.Add(columnName, _dataStructure.GetColumnType(columnName));
                rowValues.Add(columnName, "");
            }

            model.ColumnTypes = columnTypes;
            model.RowValues = rowValues;
            model.KlasaDecyzyjna = klasaDecyzyjnaDropdown;


            return View("ClassificationMode", model);

        }


        [HttpPost]
        public IActionResult ClassificationMode(ClassificationModeVm model)
        {
            string newObjectClass = _classificationService.ClassifyNewObject(model);
            Dictionary<string, string> newRow = new Dictionary<string, string>(model.RowValues);
            newRow.Add(model.KlasaDecyzyjna, newObjectClass);
            _dataStructure.AddRow(newRow);

            return RedirectToAction("Index", "Home");
        }
    }
}
