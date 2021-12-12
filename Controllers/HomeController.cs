using EDProject1.Algorithm;
using EDProject1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDProject1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataStructure _dataStructure;
        private readonly AlgorithmProcessor _algorithmProcessor;
        public HomeController(DataStructure dataStructure, AlgorithmProcessor algorithmProcessor)
        {
            _dataStructure = dataStructure;
            _algorithmProcessor = algorithmProcessor;
        }

        public IActionResult Index()
        {
           
            DataTableVm dataTableVm = new DataTableVm
            {
                ColumnNames = _dataStructure.GetColumnNames(),
                Rows = _dataStructure.GetRowsRaw()
            };

            return View(dataTableVm);
        }
        
        public IActionResult AllStep()
        {
            _algorithmProcessor.Run();

            return RedirectToAction("Index");
        }
        public IActionResult NextStep()
        {
            _algorithmProcessor.RunOneStep();

            return RedirectToAction("Index");
        }

        public IActionResult GetVectors()
        {
            _algorithmProcessor.CreateBinaryVectors();


            string vectors = string.Join('\n', AlgorithmProcessor.Vectors.Select(x =>string.Join(',', x.Select(y => y.ToString().Replace("True","1").Replace("False", "0")))));

            return File(Encoding.UTF8.GetBytes(vectors), "text/plain", "Vectors.txt");
        }
    }
}
