using Avalonia.Controls;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Port_Scanner.Models
{
    internal class ExportToExcel
    {
        public void ExportDataGridToExcel(DataGrid dataGrid)
        {
            var items = dataGrid.ItemsSource; // Get the items displayed in the datagrid
            if (items == null) // Exit method if no items exist
                return;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"Port Scan {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);

            using (var workbook = new XLWorkbook()) // Create a new Excel workbook
            {
                // Add a new worksheet
                var worksheet = workbook.Worksheets.Add("Ports Scanned");

                // Cast the items to an IEnumerable object collection to iterate over
                var enumerable = items as IEnumerable<object>;
                if (enumerable == null) // If casting fails, exit method
                    return;

                var firstItem = enumerable.FirstOrDefault(); // Get the first item from the collection
                if (firstItem == null)
                    return;

                var properties = firstItem.GetType().GetProperties(); // Get all properties of the item for the headers

                // Write headers
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;
                }

                // Write each item's property values as a new row in the worksheet
                int row = 2; // row 2 as row 1 has the headers
                foreach (var item in enumerable)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        // Get the value of each property for the current item
                        var value = properties[col].GetValue(item);
                        // Write the value to the cell otherwise empty if null
                        worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
                    }
                    row++; // Move to the next row
                }

                workbook.SaveAs(filePath); // Saves as an Excel spreadsheet to the specified file path
            }
        }
    }
}