using EDProject1.Algorithm;
using EDProject1.Enums;
using EDProject1.Models;
using EDProject1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDProject1.Services
{
    public class ClassificationService : IClassificationService
    {
        private readonly DataStructure _dataStructure;

        public ClassificationService(DataStructure dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public string ClassifyNewObject(ClassificationModeVm model)
        {
            foreach (var prosta in AlgorithmProcessor._proste)
            {
                if(!prosta.Kierunek && prosta.WartoscCiecia <= double.Parse(model.RowValues[prosta.NazwaZmiennej]))
                {
                    return prosta.OdcietaKlasaDecyzyjna;
                }

                if(prosta.Kierunek && prosta.WartoscCiecia >= double.Parse(model.RowValues[prosta.NazwaZmiennej]))
                {
                    return prosta.OdcietaKlasaDecyzyjna;
                }
            }


            throw new Exception("Coś jest nie tak");

        }

        private List<List<decimal>> GetNumericalRawData()
        {
            List<string> numericalColumnNames = _dataStructure.GetColumnNames(ColumnType.DECIMAL, ColumnType.INT);
            List<List<decimal>> result = new();

            foreach (var row in _dataStructure.GetRowsRawWithHeaders())
            {
                List<decimal> values = new();
                foreach (var numericalColumnName in numericalColumnNames)
                    values.Add(decimal.Parse(row[numericalColumnName]));

                result.Add(values);
            }

            return result;
        }
    }
}
