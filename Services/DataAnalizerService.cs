using EDProject1.Models;
using EDProject1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDProject1.Services
{
    public class DataAnalizerService : IDataAnalizerService
    {
        private readonly DataStructure _dataStructure;

        public DataAnalizerService(DataStructure dataStructure)
        {
            _dataStructure = dataStructure;
        }

        private static List<decimal> GetOrderedIntervalBorders(decimal min, decimal max, int intervalCount)
        {
            List<decimal> result = new List<decimal> { min };
            decimal differential = (max - min) / intervalCount;

            for (int i = 1; i < intervalCount; i++)
            {
                decimal previousBorder = result[i - 1];
                result.Add(previousBorder + differential);
            }

            result.Add(max);
            return result;
        }


        public List<Point2D> GetDataForChart2D(string xName, string yName, string className)
        {
            var rowsRawWithHeders = _dataStructure.GetRowsRawWithHeaders();
            List<Point2D> result = new List<Point2D>();
            foreach (var row in rowsRawWithHeders)
            {
                Point2D point2D = new Point2D
                {
                    X = decimal.Parse(row[xName]),
                    Y = decimal.Parse(row[yName]),
                    Class = row[className]
                };
                result.Add(point2D);
            }

            return result;
        }

    }
}
