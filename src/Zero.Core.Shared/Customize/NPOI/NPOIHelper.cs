using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Zero.Customize.NPOI
{
    public static class NPOIHelper
    {
         public static ICellStyle CellStyle(ISheet sheet, 
             ZEROEnums.DataType dataType = ZEROEnums.DataType.Default, 
             bool isBold = false, bool isItalic = false, bool isUnderline = false,
             string fontName = "Times New Roman", int fontHeight = 12,
             bool border = false,
             ZEROEnums.ReportTextHAlign hAlign = ZEROEnums.ReportTextHAlign.Center,
             ZEROEnums.ReportTextVAlign vAlign = ZEROEnums.ReportTextVAlign.Middle)
        {
            // Style font
            var defaultFont = (XSSFFont) sheet.Workbook.CreateFont();
            defaultFont.FontHeightInPoints = fontHeight;
            defaultFont.FontName = fontName;
            defaultFont.Color = IndexedColors.Black.Index;
            defaultFont.IsBold = isBold;
            defaultFont.IsItalic = isItalic;
            
            // Style Border
            var cellStyle = sheet.Workbook.CreateCellStyle();
            
            cellStyle.BorderBottom = border ? BorderStyle.Thin : BorderStyle.None;
            cellStyle.BorderLeft = border ? BorderStyle.Thin : BorderStyle.None;
            cellStyle.BorderRight = border ? BorderStyle.Thin : BorderStyle.None;
            cellStyle.BorderTop = border ? BorderStyle.Thin : BorderStyle.None;
            
            if (hAlign == ZEROEnums.ReportTextHAlign.Left)
                cellStyle.Alignment = HorizontalAlignment.Left;
            else if (hAlign == ZEROEnums.ReportTextHAlign.Right)
                cellStyle.Alignment = HorizontalAlignment.Right;
            else
                cellStyle.Alignment = HorizontalAlignment.Center;
            
            if (vAlign == ZEROEnums.ReportTextVAlign.Top)
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
            else if (vAlign == ZEROEnums.ReportTextVAlign.Bottom)
                cellStyle.VerticalAlignment = VerticalAlignment.Bottom;
            else 
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
            
            if (dataType == ZEROEnums.DataType.Decimal)
                cellStyle.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0;-#,##0");
            
            cellStyle.SetFont(defaultFont);
            return cellStyle;
        }
    }
}