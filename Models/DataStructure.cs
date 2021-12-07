using EDProject1.Algorithm;
using EDProject1.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EDProject1.Models
{
    public class DataStructure
    {
        private readonly Dictionary<string, ColumnType> _columnTypes = new Dictionary<string, ColumnType>();
        private List<DataRow> _rows = new();
        private readonly List<DataRow> _rowsOriginal = new();

        public void InitializeColumnTypes(IEnumerable<string> columnNames)
        {
            foreach (var name in columnNames)
            {
                _columnTypes.Add(name, ColumnType.INT);
            }
        }

        public List<Dictionary<string, string>> GetRowsRawWithHeaders()
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            List<List<string>> rowsRaw = GetRowsRaw();

            List<string> columnNames = GetColumnNames();
            foreach (var row in rowsRaw)
            {
                Dictionary<string, string> rowRaw = new Dictionary<string, string>();
                for (int i = 0; i < columnNames.Count; i++)
                {
                    rowRaw.Add(columnNames[i], row[i]);
                }

                result.Add(rowRaw);
            }

            return result;
        }

        public List<Dictionary<string, string>> GetOriginalRowsRawWithHeaders()
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            List<List<string>> rowsRaw = GetOriginalRowsRaw();

            List<string> columnNames = GetColumnNames();
            foreach (var row in rowsRaw)
            {
                Dictionary<string, string> rowRaw = new Dictionary<string, string>();
                for (int i = 0; i < columnNames.Count; i++)
                {
                    rowRaw.Add(columnNames[i], row[i]);
                }

                result.Add(rowRaw);
            }

            return result;
        }

        public List<List<string>> GetOriginalRowsRaw()
        {
            List<List<string>> result = new List<List<string>>();

            foreach (DataRow row in _rowsOriginal)
            {
                result.Add(row.GetRawData());
            }

            return result;
        }

        public List<string> GetColumnNames(params ColumnType[] columnTypes)
        {
            var result = _columnTypes.Keys.ToList();
            if (columnTypes?.Any() == true)
                result = _columnTypes.Where(w => columnTypes.Contains(w.Value)).Select(s => s.Key).ToList();
            return result;
        }

        public List<List<string>> GetRowsRaw()
        {
            List<List<string>> result = new List<List<string>>();

            foreach (DataRow row in _rows)
            {
                result.Add(row.GetRawData());
            }

            return result;
        }

        public List<string> GetRowsRawForColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            List<string> toReturn = new List<string>();
            if (_columnTypes.ContainsKey(columnName))
                toReturn = _rows.Select(s => s.GetValue(columnName)).ToList();

            return toReturn;
        }

        public IEnumerable<object> GetValuesDistinct(string columnName)
        {
            return _rows.Select(x => x.GetValue(columnName)).Distinct();
        }

        public IEnumerable<object> GetOriginalValuesDistinct(string columnName)
        {
            return _rowsOriginal.Select(x => x.GetValue(columnName)).Distinct();
        }

        public void ImportData(DataTable dataTable)
        {
            foreach (System.Data.DataRow item in dataTable.Rows)
            {

                Dictionary<string, string> _cells = new Dictionary<string, string>();

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    _cells.Add(dataTable.Columns[i].ColumnName, item[dataTable.Columns[i].ColumnName].ToString());
                }

                AddRow(_cells);
            }

        }

        public void AddRow(Dictionary<string, string> values)
        {
            Dictionary<string, string> cells = new Dictionary<string, string>();
            foreach (var column in _columnTypes)
            {
                if (values.TryGetValue(column.Key, out string cellValue))
                {
                    cells[column.Key] = cellValue;
                    UpdateColumnType(column.Key, cellValue);
                }
                else
                {
                    cells[column.Key] = "";
                    UpdateColumnType(column.Key, cellValue);
                }
            }

            _rows.Add(new DataRow(cells));
            _rowsOriginal.Add(new DataRow(cells));
        }

        public void AddColumn(string columnName, List<string> values)
        {
            if (!_columnTypes.ContainsKey(columnName))
            {
                _columnTypes.Add(columnName, ColumnType.INT);

                for (int i = 0; i < _rows.Count; i++)
                {
                    string cellValue = values.ElementAtOrDefault(i) ?? string.Empty;
                    _rows[i].AddCell(columnName, cellValue);
                    UpdateColumnType(columnName, cellValue);
                }
            }
        }

        public object GetValue(int rowNumber, string columnName)
        {
            return _rows.ElementAtOrDefault(rowNumber)?.GetValue(columnName);
        }

        public ColumnType GetColumnType(string columnName)
        {
            if (_columnTypes.TryGetValue(columnName, out ColumnType columnType))
                return columnType;

            throw new Exception($"Column {columnName} not exists in data structure.");
        }

        public void OrderByColumn(string columnName)
        {
            _rows = _rows.OrderBy(x=>Double.Parse(x.GetValue(columnName).Replace('.',','))).ToList();
        }

        public void RemoveFirstRows(int number)
        {
            _rows = _rows.Skip(number).ToList();
        }

        public void RemoveFirstWithClass(string className, string orderColumn, int number)
        {
            OrderByColumn(orderColumn);
            var rowsToRemove = _rows.Where(x => x.GetRawData().Last() == className).Take(number).ToList();
            AlgorithmProcessor._usunietePunkty.AddRange(rowsToRemove.Select(x=>x.GetRawDataWithHeaders()));
            _rows = _rows.Where(x => !rowsToRemove.Contains(x)).ToList();
           
        }

        public void RemoveLastWithClass(string className, string orderColumn, int number)
        {
            OrderByColumn(orderColumn);
            var rowsToRemove = _rows.Where(x => x.GetRawData().Last() == className).TakeLast(number).ToList();
            AlgorithmProcessor._usunietePunkty.AddRange(rowsToRemove.Select(x => x.GetRawDataWithHeaders()));
            _rows = _rows.Where(x => !rowsToRemove.Contains(x)).ToList();
        }

        public void RemoveLastRows(int number)
        {
            _rows = _rows.Take(_rows.Count - number).ToList();
        }

        private void UpdateColumnType(string columnName, string value)
        {
            if (!int.TryParse(value, out _))
                _columnTypes[columnName] = ColumnType.DECIMAL;
            if (!decimal.TryParse(value, out _))
                _columnTypes[columnName] = ColumnType.STRING;
        }
    }
}
