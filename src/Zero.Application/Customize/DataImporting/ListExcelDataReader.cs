using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Castle.Core.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Zero.DataExporting.Excel.NPOI;
using Zero.Interfaces;

namespace Zero.Customize.DataImporting
{
    public class ListExcelDataReader<T> : NpoiExcelImporterBase<T>, IListExcelDataReader<T> where T : new()
    {
        public ILogger Logger { get; set; }

        public ListExcelDataReader(ILogger logger)
        {
            Logger = logger;
        }
        
        public List<T> GetFromExcel(byte[] fileBytes)
        {
            return CustomProcessExcelFile(fileBytes);
        }

        private List<T> CustomProcessExcelFile(byte[] fileBytes)
        {
            Stream stream = new MemoryStream(fileBytes);
            ISheet sheet = null;
            IWorkbook workbook;
            try
            {
                workbook = new XSSFWorkbook(stream);
                sheet = workbook.GetSheetAt(0);
            }
            catch (Exception eXlsx)
            {
                Logger.Error("Importing Reader XLSX failed!", eXlsx);
                try
                {
                    stream.Position = 0;
                    workbook = new HSSFWorkbook(stream);
                    sheet = workbook.GetSheetAt(0);
                }
                catch (Exception eXls)
                {
                    Logger.Error("Importing Reader XLS Failed", eXls);
                }
            }

            if (sheet != null)
            {
                var res = new List<T>();
                var headerRow = sheet.GetRow(2);
                int cellCount = headerRow.LastCellNum;

                for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    var objNew = new T();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) continue;

                        var displayName = ImportExcelHelper.TranslateColumnIndexToName(j);
                        var myType = objNew.GetType();
                        var piShared = myType.GetProperty(ImportExcelHelper.GetName<T>(displayName));

                        if (piShared == null || string.IsNullOrWhiteSpace(piShared.Name)) continue;

                        try
                        {
                            var value = ImportExcelHelper.GetValueFormCell(cell);
                            var type = piShared.PropertyType.Name.ToLower();
                            switch (type)
                            {
                                case "double":
                                    value = string.IsNullOrEmpty(value) ? "0" : value;
                                    piShared.SetValue(objNew, Convert.ToDouble(value), null);
                                    break;
                                case "decimal":
                                    value = string.IsNullOrEmpty(value) ? "0" : value;
                                    piShared.SetValue(objNew, Convert.ToDecimal(value), null);
                                    break;
                                case "datetime":
                                    piShared.SetValue(objNew,
                                        DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                        null);
                                    break;
                                case "boolean":
                                    value = string.IsNullOrEmpty(value) ? "0" : value;
                                    piShared.SetValue(objNew, value == "1", null);
                                    break;
                                case "int32":
                                    piShared.SetValue(objNew, Convert.ToInt32(value), null);
                                    break;
                                default:
                                    piShared.SetValue(objNew, value, null);
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    res.Add(objNew);
                }
                return res;
            }
            return null;
        }
    }
}