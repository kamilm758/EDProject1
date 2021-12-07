using System.Collections.Generic;


namespace EDProject1.Models
{
    public class Chart2DVm
    {
        public string XColumnName { get; set; }
        public string YColumnName { get; set; }
        public decimal Xmax { get; set; }
        public decimal YMax { get; set; }
        public decimal Xmin { get; set; }
        public decimal Ymin { get; set; }
        public string ClassName { get; set; }
        public Dictionary<string, string> ClassColorMapping { get; set; }
        public List<Point2D> ClassPoints { get; set; }
        public List<Prosta> Proste { get; set; }
    }
}
