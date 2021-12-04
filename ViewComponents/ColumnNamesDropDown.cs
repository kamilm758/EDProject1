using EDProject1.Enums;
using EDProject1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDProject1.ViewComponents
{
    public class ColumnNamesDropDown : ViewComponent
    {
        private readonly DataStructure _dataStructure;

        public ColumnNamesDropDown(DataStructure dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public IViewComponentResult Invoke(string dropdownId, List<ColumnType> columnTypes)
        {
            ViewBag.DropdownId = dropdownId;
            var columnNames = columnTypes?.SelectMany(sm => _dataStructure.GetColumnNames(sm)).ToList() ?? new List<string>();
            return View(columnNames);
        }
    }
}
