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
        public static List<Prosta> _proste = new();
        public static List<Dictionary<string, string>> _usunietePunkty = new();
        public static List<List<bool>> Vectors = new();

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
                    try
                    {
                       
                        _rzutowanieNaProsta[value.Key].Wymiar.Add((Double.Parse(value.Value.Replace('.',',')), rowRaw.Last().Value));
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                      
                    }
                }

                foreach (var item in _rzutowanieNaProsta)
                {
                    _rzutowanieNaProsta[item.Key].Wymiar = item.Value.Wymiar.OrderBy(x => x.Item1).ToList();
                }
         
      
          
        }

        public void RunOneStep()
        {
            if (_dataStructure.GetRowsRaw().Count == 0) return;

            RzutujNaProsta();

            var nextCut = NextCut();
            if (nextCut.Item2 == 0)
            {
              RemoveSomeRows();
                RzutujNaProsta();
                nextCut = NextCut();
              
            }
    
            
            _dataStructure.OrderByColumn(nextCut.Item1);
            double miesjceCiecia;
            List<Dictionary<string,string>> rowsRaw = _dataStructure.GetRowsRawWithHeaders();

            if (nextCut.Item3)
            {
               miesjceCiecia = double.Parse(rowsRaw.Take(nextCut.Item2).Last()[nextCut.Item1]);

                _dataStructure.RemoveFirstRows(nextCut.Item2);
            }
            else
            {
                miesjceCiecia = double.Parse(rowsRaw.TakeLast(nextCut.Item2).First()[nextCut.Item1]);
                _dataStructure.RemoveLastRows(nextCut.Item2);
            }
            

            _proste.Add(new Prosta(nextCut.Item3, miesjceCiecia, nextCut.Item1));
            
        }

        public void Run()
        {
           
            while(_dataStructure.GetRowsRaw().Any())
            {
                RunOneStep();
            }
        }

        private void RemoveSomeRows()
        {
           List<string> columnNames = _dataStructure.GetColumnNames();
            (string, string, int, bool) kierunekUsunieciaPunktow = new("", "", int.MaxValue, false);
            foreach (var columnName in columnNames.SkipLast(1))
            {
                _dataStructure.OrderByColumn(columnName);
                List<Dictionary<string,string>> rawRows = _dataStructure.GetRowsRawWithHeaders();
                List<Dictionary<string, string>> rawRowsLikeFirstValue = new(); 

                string firstValue = rawRows.First()[columnName];
                rawRowsLikeFirstValue = rawRows.Where(x => x[columnName] == firstValue).ToList();

                Dictionary<string, int> classCount = new();
                
                foreach (var item in rawRowsLikeFirstValue)
                {
                    string className = item[columnNames.Last()];
                    if (!classCount.ContainsKey(className))
                        classCount.Add(className, 0);

                    classCount[className]++;
                }

                var lowestCountClass = classCount.OrderBy(x => x.Value).First();
                if (lowestCountClass.Value < kierunekUsunieciaPunktow.Item3)
                    kierunekUsunieciaPunktow = (columnName, lowestCountClass.Key, lowestCountClass.Value, true);


                List<Dictionary<string, string>> rawRowsLikeLastValue = new();
                string LastValue = rawRows.Last()[columnName];
                rawRowsLikeLastValue = rawRows.Where(x => x[columnName] == LastValue).ToList();

                Dictionary<string, int> classCountLast = new();

                foreach (var item in rawRowsLikeLastValue)
                {
                    string className = item[columnNames.Last()];
                    if (!classCountLast.ContainsKey(className))
                        classCountLast.Add(className, 0);

                    classCountLast[className]++;
                }

                var lowestCountClassLast = classCountLast.OrderBy(x => x.Value).First();
                if (lowestCountClassLast.Value < kierunekUsunieciaPunktow.Item3)
                    kierunekUsunieciaPunktow = (columnName, lowestCountClassLast.Key, lowestCountClassLast.Value, false);
            }


            if (kierunekUsunieciaPunktow.Item4)
                _dataStructure.RemoveFirstWithClass(kierunekUsunieciaPunktow.Item2, kierunekUsunieciaPunktow.Item1, kierunekUsunieciaPunktow.Item3);
            else
                _dataStructure.RemoveLastWithClass(kierunekUsunieciaPunktow.Item2, kierunekUsunieciaPunktow.Item1, kierunekUsunieciaPunktow.Item3);

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

        public void CreateBinaryVectors()
        {
            List<Dictionary<string, string>> rowsRaw = _dataStructure.GetOriginalRowsRawWithHeaders();

            foreach (var row in rowsRaw)
            {
                List<bool> binaryVector = new();
                foreach (var prosta in _proste)
                {
                    double rowValue = double.Parse(row[prosta.NazwaZmiennej].Replace('.', ','));
                    if (prosta.Kierunek && rowValue <= prosta.WartoscCiecia)
                        binaryVector.Add(true);
                    else if (!prosta.Kierunek && rowValue > prosta.WartoscCiecia)
                        binaryVector.Add(true);
                    else
                        binaryVector.Add(false);


                }
                Vectors.Add(binaryVector);
            }
        }
    }
}
