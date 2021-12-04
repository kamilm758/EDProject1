using EDProject1.Models;
using EDProject1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EDProject1.Services
{
    public class ImportService : IImportService
    {

        public ImportService()
        {

        }

        public DataTable ImportData(UploadFileModel model)
        {
            string fileName = Path.GetExtension(model.File.FileName);
            if (fileName == ".xlsx")
            {
                //return ImportExcelData(file);
            }
            else if (fileName == ".txt")
            {
                return ImportTextData(model);
            }
            throw new Exception($"{fileName} is not supported");
        }

        public DataTable ImportExcelData(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public static bool IsAWord(string text)
        {
            var regex = new Regex(@"\b[\w']+\b");
            var match = regex.Match(text);
            return match.Value.Equals(text);
        }

        public DataTable ImportTextData(UploadFileModel model)
        {
            DataTable dt = new DataTable();

            if (model.File.Length > 0)
            {
                StreamReader stream = new StreamReader(model.File.OpenReadStream());

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    if (!(line.StartsWith("#") || String.IsNullOrEmpty(line.Trim())))
                    {

                        if (dt.Columns.Count == 0 && model.IsHeader)
                        {
                            string[] columnHeders = line.Split("\t");
                            foreach (var item in columnHeders)
                            {
                                dt.Columns.Add(item);
                            }
                            continue;
                        }
                        if (dt.Columns.Count == 0 && !model.IsHeader)
                        {
                            string[] columnHeders = line.Split("\t");
                            int columnNumbers = columnHeders.Count();

                            for (int i = 1; i <= columnNumbers; i++)
                            {
                                dt.Columns.Add($"Column{i}");
                            }
                        }


                        var row = dt.NewRow();
                        string[] cells = line.Split("\t");

                        for (int i = 0; i < cells.Length; i++)
                        {
                            row[i] = cells[i];
                        }
                        dt.Rows.Add(row);

                    }
                }
            }
            return dt;
        }
    }
}
