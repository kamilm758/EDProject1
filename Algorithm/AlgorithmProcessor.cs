using EDProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDProject1.Algorithm
{
    public class AlgorithmProcessor
    {
        private readonly DataStructure _dataStructure;
        private readonly Dictionary<string, RzutownieNaProsta> _rzutowanieNaProsta;

        public AlgorithmProcessor(DataStructure dataStructure)
        {
            _dataStructure = dataStructure;
            _rzutowanieNaProsta = new();
        }

        public void RzutujNaProsta()
        {
            List<string> columnNames = _dataStructure.GetColumnNames();
            _rzutowanieNaProsta.Clear();
            foreach (var columnName in columnNames.SkipLast(1))
            {
                _rzutowanieNaProsta.Add(columnName, new RzutownieNaProsta());
            }

            foreach (Dictionary<string, string> rowRaw in _dataStructure.GetRowsRawWithHeaders())
            {
                foreach (var value in rowRaw.SkipLast(1))
                {
                    _rzutowanieNaProsta[value.Key].Wymiar.Add((Double.Parse(value.Value), rowRaw.Last().Value));
                }
            }

            foreach (var item in _rzutowanieNaProsta)
            {
                _rzutowanieNaProsta[item.Key].Wymiar = item.Value.Wymiar.OrderBy(x =>x.Item1).ToList();
            }
          
        }

        public void RunOneStep()
        {
            RzutujNaProsta();

            var nextCut = NextCut();
            _dataStructure.OrderByColumn(nextCut.Item1);
            if (nextCut.Item3)
                _dataStructure.RemoveFirstRows(nextCut.Item2);
            else
                _dataStructure.RemoveLastRows(nextCut.Item2);
        }

        public void Run()
        {
           
            while(_dataStructure.GetRowsRaw().Any())
            {
                RunOneStep();
            }
             

        }

        private (string, int, bool) NextCut()
        {
            List<(string, int, bool)> prepareToCut = new();   


            foreach (var value in _rzutowanieNaProsta)
            {
                var firstValue = _rzutowanieNaProsta[value.Key].Wymiar.First();

                (string, int, bool) toAdd = ((value.Key, 0, true));
                foreach (var item in _rzutowanieNaProsta[value.Key].Wymiar)
                {
                    List<(double, string)> podzbiorZTaSamoWartoscia = _rzutowanieNaProsta[value.Key].Wymiar.Where(x => x.Item1 == item.Item1).ToList();

                    if (podzbiorZTaSamoWartoscia.All(x=>x.Item2 == firstValue.Item2))
                        toAdd.Item2++;
                    else break;
                }
                prepareToCut.Add(toAdd);

                toAdd = ((value.Key, 0, false));
                var lastValue = _rzutowanieNaProsta[value.Key].Wymiar.Last();
                foreach (var item in _rzutowanieNaProsta[value.Key].Wymiar.AsEnumerable().Reverse())
                {
                    List<(double, string)> podzbiorZTaSamoWartoscia = _rzutowanieNaProsta[value.Key].Wymiar.Where(x => x.Item1 == item.Item1).ToList();

                    if (podzbiorZTaSamoWartoscia.All(x => x.Item2 == lastValue.Item2))
                        toAdd.Item2++;
                    else break;
                }
                prepareToCut.Add(toAdd);
            }

            return prepareToCut.OrderBy(x=>x.Item2).Last();

        }
    }
}
