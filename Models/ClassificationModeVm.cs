using EDProject1.Enums;
using System.Collections.Generic;

namespace EDProject1.Models
{
    public class ClassificationModeVm
    {
        public Dictionary<string, ColumnType> ColumnTypes { get; set; }
        public Dictionary<string, string> RowValues { get; set; }
        public string KlasaDecyzyjna { get; set; }
    }
}
