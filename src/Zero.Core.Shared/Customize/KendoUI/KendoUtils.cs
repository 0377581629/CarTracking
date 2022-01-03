using System;
using System.Collections.Generic;
using System.Linq;
using Zero.KendoUI;

namespace Zero.Customize.KendoUI
{
   public static class KendoUtils
    {
        public static string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        public static KendoModels.GeneralImportDto SpreadSheetImport(KendoModels.GeneralImportModel input)
        {
            var res = new KendoModels.GeneralImportDto
            {
                Name = input.ImportName
            };
            var sheet = new KendoModels.SpreadSheet
            {
                name = input.ImportName
            };

            var lstRow = new List<KendoModels.SpreadSheet_Row>();
            var lstCol = new List<KendoModels.SpreadSheet_Column>();
            var headerRowCount = 0;

            var lstHeader = new List<KendoModels.SpreadSheet_Cell_Header>();
            var lstHeaderCode = new List<KendoModels.SpreadSheet_Cell_Header>();

            if (input.ListField.Any())
            {
                var colIndex = 0;
                var rowIndex = 0;

                // HEADER
                foreach (var header in input.ListField)
                {
                    lstHeader.Add(new KendoModels.SpreadSheet_Cell_Header
                    {
                        width = header.Width,
                        value = header.Header,
                        index = colIndex,
                        background = KendoConsts.HeaderBackground,
                        textAlign = KendoConsts.TextAlign.Center,
                        bold = true
                    });
                    lstCol.Add(new KendoModels.SpreadSheet_Column
                    {
                        index = colIndex,
                        width = 50 + header.Width
                    });
                    colIndex++;
                    
                }
                headerRowCount++;
                
                var headerRow = new KendoModels.SpreadSheet_Row
                {
                    index = rowIndex,
                    height = 50 + input.HeaderHeight
                };
                rowIndex++;
                headerRow.cells.AddRange(lstHeader);
                lstRow.Add(headerRow);

                // HEADER CODE
                if (input.HeaderCode)
                {
                    colIndex = 0;

                    foreach (var header in input.ListField)
                    {
                        lstHeaderCode.Add(new KendoModels.SpreadSheet_Cell_Header
                        {
                            width = header.Width,
                            value = header.HeaderCode,
                            index = colIndex,
                            background = KendoConsts.HeaderBackground,
                            textAlign = KendoConsts.TextAlign.Center,
                            bold = true
                        });
                        colIndex++;
                    }

                    var headerCodeRow = new KendoModels.SpreadSheet_Row
                    {
                        index = rowIndex,
                        height = 25 + input.HeaderCodeHeight
                    };
                    headerCodeRow.cells.AddRange(lstHeaderCode);
                    rowIndex++;
                    headerRowCount++;
                    lstRow.Add(headerCodeRow);
                }

                // Row Input
                var inputCells = new List<KendoModels.SpreadSheet_Cell>();
                for (var i = rowIndex; i < Math.Abs(input.RowCount + 50 + rowIndex); i++)
                {
                    var inputRow = new KendoModels.SpreadSheet_Row
                    {
                        index = i,
                        height = 25 + input.InputHeight
                    };
                    colIndex = 0;

                    if (i > 0)
                    {
                        foreach (var field in input.ListField)
                        {
                            var numberFormat = "";
                            var fullNumberFormat = "";
                            var isNumberCol = field.DataType == (int) ZeroEnums.DataType.Number;
                            if (isNumberCol)
                            {
                                if (field.FractionalLetterCount > 0)
                                {
                                    numberFormat = "0.".PadRight(field.FractionalLetterCount + 2, '0');
                                    fullNumberFormat =
                                        $"[Black]#,##{numberFormat};[Black](#,##{numberFormat});[Blue]\"\";[Magenta]@";
                                }
                                else
                                {
                                    fullNumberFormat = "[Black]#,##;[Black](#,##);[Blue]\"\";[Magenta]@";
                                }
                            }

                            inputCells.Add(new KendoModels.SpreadSheet_Cell
                            {
                                index = colIndex,
                                enable = true,
                                format = !isNumberCol ? null : fullNumberFormat,
                                color = isNumberCol ? "" : KendoConsts.DefaultColor,
                                textAlign = isNumberCol ? KendoConsts.TextAlign.Right : KendoConsts.TextAlign.Left
                            });
                            colIndex++;
                        }
                    }

                    inputRow.cells.AddRange(inputCells);
                    lstRow.Add(inputRow);
                }
            }

            sheet.rows = lstRow;
            sheet.columns = lstCol;
            sheet.frozenRows = headerRowCount;

            res.Sheets = new List<KendoModels.SpreadSheet> {sheet};
            res.ColCount = lstCol.Count();
            res.RowCount = lstRow.Count();
            return res;
        }

        public static void AddListHeader(List<KendoModels.SpreadSheet_Cell_Header> colList,
            ref KendoModels.SpreadSheet_Row row,
            ref List<KendoModels.SpreadSheet_Column> lstCol, ref int colIndex)
        {
            foreach (var col in colList)
            {
                AddCell(ref row, colIndex, col.value,
                    KendoConsts.HeaderFormat,
                    KendoConsts.HeaderBackground,
                    KendoConsts.HeaderColor,
                    KendoConsts.HeaderTextAlign,
                    KendoConsts.VAlign,
                    KendoConsts.HeaderBold,
                    KendoConsts.HeaderWrap,
                    KendoConsts.HeaderFontSize,
                    KendoConsts.HeaderEnable,
                    null);

                AddCol(ref lstCol, colIndex, col.width);
                colIndex++;
            }
        }

        public static void AddListHeader(Dictionary<string, int> colList,
            ref KendoModels.SpreadSheet_Row row,
            ref List<KendoModels.SpreadSheet_Column> lstCol, ref int colIndex)
        {
            foreach (var col in colList)
            {
                AddCell(ref row, colIndex, col.Key,
                    KendoConsts.HeaderFormat,
                    KendoConsts.HeaderBackground,
                    KendoConsts.HeaderColor,
                    KendoConsts.HeaderTextAlign,
                    KendoConsts.VAlign,
                    KendoConsts.HeaderBold,
                    KendoConsts.HeaderWrap,
                    KendoConsts.HeaderFontSize,
                    KendoConsts.HeaderEnable,
                    null);

                AddCol(ref lstCol, colIndex, col.Value);
                colIndex++;
            }
        }

        public static void AddListHeaderNumber(Dictionary<string, int> listTenCot, ref KendoModels.SpreadSheet_Row row)
        {
            var colIndex = 0;
            foreach (var col in listTenCot)
            {
                AddCell(ref row, colIndex, "( " + colIndex + " )", "text", "#bfbfbf", "#000000", "center", "center",
                    "true", true, 14, false, null);
                colIndex++;
            }
        }

        private static void AddCol(ref List<KendoModels.SpreadSheet_Column> lstCol, int index, int width)
        {
            var col = new KendoModels.SpreadSheet_Column
            {
                index = index,
                width = width
            };
            lstCol.Add(col);
        }

        public static void AddCell(ref KendoModels.SpreadSheet_Row row, int colIndex, object value, string format,
            string background, string color,
            string textalign, string verticalAlign, string bold, bool wrap, double fontSize, bool enable, string formula)
        {
            if (row.cells == null) row.cells = new List<KendoModels.SpreadSheet_Cell>();
            var cell = new KendoModels.SpreadSheet_Cell
            {
                index = colIndex,
                value = value,
                format = format,
                background = background,
                color = color,
                textAlign = textalign,
                verticalAlign = verticalAlign,
                bold = Convert.ToBoolean(bold),
                wrap = wrap,
                fontSize = fontSize,
                enable = enable,
                formula = formula
            };
            row.cells.Add(cell);
        }

        public static void CleanCell(ref KendoModels.SpreadSheet_Row row)
        {
            foreach (var c in row.cells)
            {
                if (c.value == null)
                    c.value = "";
                var temp = c.value.ToString();
                temp = temp.Trim().Replace(Environment.NewLine, "");
                c.value = temp;
            }
        }
    }
}