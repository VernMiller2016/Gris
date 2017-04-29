﻿using GRis.Core.Extensions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace GRis.Core.Utils
{
    public static class ImportUtils
    {
        /// <summary>
        /// Convert stream (of excel sheet) into datatable, which columns will match the columns defined in the excel sheet.
        /// </summary>
        /// <param name="inputStream">The Stream object of the excel sheet</param>
        /// <param name="hasHeader">A flag indicates if the excel sheet has a headers row or not</param>
        /// <returns>A Datatable represents the excel sheet</returns>
        public static DataTable ImportXlsxToDataTable(Stream inputStream, bool hasHeader, bool skipEmptyRow = true)
        {
            var dt = new DataTable();
            using (var excel = new ExcelPackage(inputStream))
            {
                var ws = excel.Workbook.Worksheets.First();
                // add DataColumns to DataTable
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    dt.Columns.Add(hasHeader
                        ? firstRowCell.Text
                        : String.Format("Column {0}", firstRowCell.Start.Column));

                // add DataRows to DataTable
                int startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = dt.NewRow();
                    foreach (var cell in wsRow)
                        row[cell.Start.Column - 1] = cell.Text;

                    if (skipEmptyRow)
                    {
                        if (!row.AreAllColumnsNullOrEmpty())
                            dt.Rows.Add(row);
                    }
                    else
                    {
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Convert stream (of excel sheet) into datatable, with specific set of columns.
        /// So, it will import only the required passed columns.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="columnsToImport"></param>
        /// <returns></returns>
        public static DataTable ImportXlsxToDataTable(Stream inputStream, params string[] columnsToImport)
        {
            var dt = new DataTable();
            const bool hasHeader = true; // it must be true, as we are importing specific columns.
            var mappingColIndexToColHeader = new Dictionary<int, string>();
            using (var excel = new ExcelPackage(inputStream))
            {
                var ws = excel.Workbook.Worksheets.First();
                // add DataColumns to DataTable
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    var columnName = (hasHeader ? firstRowCell.Text : String.Format("Column {0}", firstRowCell.Start.Column));
                    if (columnsToImport.Contains(columnName))
                    {
                        dt.Columns.Add(columnName);
                        mappingColIndexToColHeader.Add(firstRowCell.Start.Column, columnName);
                    }
                }

                // add DataRows to DataTable
                int startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = dt.NewRow();
                    foreach (var cell in wsRow)
                    {
                        if (mappingColIndexToColHeader.ContainsKey(cell.Start.Column))
                        {
                            row[mappingColIndexToColHeader[cell.Start.Column]] = cell.Text;
                        }
                    }

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// Check if a specific column(s) exist or not in a specified datatable
        /// </summary>
        /// <param name="dt">The datatable to check columns against</param>
        /// <param name="columns">List of columns to check for existence</param>
        /// <returns>List of columns that don't exist in the specified datatable</returns>
        public static List<string> ValidateColumnsExist(DataTable dt, params string[] columns)
        {
            return columns.Where(column => !dt.Columns.Contains(column)).ToList();
        }
    }
}