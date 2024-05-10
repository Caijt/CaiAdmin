using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CaiAdmin.Common
{
    public class ExcelHelper
    {
        public static List<Dictionary<string, object>> ToList(Stream stream, int startRow = 2, string[] keys = null)
        {
            List<Dictionary<string, object>> l = new List<Dictionary<string, object>>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets[1];
                int rows = sheet.Dimension.Rows;
                int cols = sheet.Dimension.Columns;

                for (int row = startRow; row <= rows; row++)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    for (int col = 1; col <= cols; col++)
                    {
                        string key;
                        if (keys != null && col <= keys.Length)
                        {
                            key = keys[col - 1];
                        }
                        else
                        {
                            key = Regex.Match(sheet.Cells[row, col].Address, "[A-Z]+").Value;
                        }
                        dict.Add(key, sheet.Cells[row, col].Value);
                    }
                    l.Add(dict);
                }
            }
            return l;
        }

        public static byte[] ListToExcel<T>(List<T> list, Dictionary<string, Func<T, object>> dict, string worksheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(worksheetName);
                //标题
                for (int col = 1; col <= dict.Count; col++)
                {
                    var cell = worksheet.Cells[1, col];
                    cell.Value = dict.Keys.ElementAt(col - 1);

                }
                //内容
                for (int row = 1; row <= list.Count; row++)
                {
                    T obj = list[row - 1];
                    for (int col = 1; col <= dict.Count; col++)
                    {
                        var cell = worksheet.Cells[row + 1, col];
                        cell.Value = dict.Values.ElementAt(col - 1).Invoke(obj);
                    }
                }
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells.Style.WrapText = true;
                worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                return package.GetAsByteArray();
            }
        }
    }

}
