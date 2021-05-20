using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abp.Dependency;
using Zero.DataExporting.Excel.NPOI;
using Zero.Dto;
using Zero.Extensions;
using Zero.Interfaces;
using Zero.Storage;

namespace Zero.Customize.DataExporting
{
    public class InvalidExporter<T> : NpoiExcelExporterBase, IInvalidExporter<T>
    {
        public InvalidExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<T> objs, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "InvalidImportObjs";

            var exportPropertiesHeader = new List<string>();
            var exportProperties = new List<string>();

            var props = typeof(T).GetProperties().Where(prop => prop.IsDefined(typeof(InvalidExportAttribute), true));
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    var invalidAttr = attr as InvalidExportAttribute;
                    if (invalidAttr == null) continue;
                    exportProperties.Add(prop.Name);
                    exportPropertiesHeader.Add(string.IsNullOrEmpty(invalidAttr.Name) ? L(prop.Name) : L(invalidAttr.Name));
                }
            }

            return CreateExcelPackage(
                $"{fileName}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L(fileName).Replace("[", "").Replace("]", ""));
                    AddHeader(sheet, exportPropertiesHeader.ToArray());
                    AddObjects(sheet, objs, exportProperties);
                    for (var i = 0; i < exportProperties.Count; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}