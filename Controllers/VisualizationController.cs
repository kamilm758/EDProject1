using EDProject1.Models;
using EDProject1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EDProject1.Controllers
{
    public class VisualizationController : Controller
    {
        private DataStructure _dataStructure;
        private readonly IDataAnalizerService _dataAnalizerService;
        public VisualizationController(DataStructure dataStructure, IDataAnalizerService dataAnalizerService)
        {
            _dataStructure = dataStructure;
            _dataAnalizerService = dataAnalizerService;
        }
        public IActionResult Show2DChart(string xAxisDropdown, string yAxisDropdown, string classDropdown)
        {
            Chart2DVm chart2DVm = new Chart2DVm
            {
                XColumnName = xAxisDropdown,
                YColumnName = yAxisDropdown,
                ClassName = classDropdown,
                ClassColorMapping = _dataStructure.GetValuesDistinct(classDropdown).Select(x => x.ToString()).ToDictionary(x => x, y => string.Empty)
            };

            return View("ColorSelecting", chart2DVm);
        }

        [HttpPost]
        public IActionResult Show2DChart(Chart2DVm model)
        {
            List<Point2D> points2D = _dataAnalizerService.GetDataForChart2D(model.XColumnName, model.YColumnName, model.ClassName);
            model.ClassPoints = points2D;

            return View(model);
        }

    }
}
