using EDProject1.Models;
using System.Collections.Generic;

namespace EDProject1.Services.Interfaces
{
    public interface IDataAnalizerService
    {

        List<Point2D> GetDataForChart2D(string xName, string yName, string className);
      
    }
}
