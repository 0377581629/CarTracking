using System.Collections.Generic;
using Zero.KendoUI;

namespace Zero.Customize.KendoUI
{
    public partial class KendoModels
    {
        #region SpreadSheet

        public class SpreadSheet
        {
            public string name { get; set; }
            public List<SpreadSheet_Row> rows { get; set; }
            public List<SpreadSheet_Column> columns { get; set; }
            public string selection { get; set; } = "A1:A1";
            public string activeCell { get; set; } = "A1:A1";
            public int frozenRows { get; set; }

            public int frozenColumns { get; set; }
            public bool showGridLines { get; set; } = true;
            public List<string> mergedCells { get; set; }
            public List<string> hyperlinks { get; set; }

            public object defaultCellStyle { get; set; } = new
            {
                fontFamily = "Arial",
                fontSize = 13,
                bold = false
            };
        }

        public class SpreadSheet_Row
        {
            public int index { get; set; }
            public int height { get; set; } = 25;
            public List<SpreadSheet_Cell> cells { get; set; } = new List<SpreadSheet_Cell>();
        }

        public class SpreadSheet_Cell
        {
            public int index { get; set; }
            public object value { get; set; }
            public string format { get; set; } = KendoConsts.DefaultFormat;
            public string color { get; set; } = KendoConsts.DefaultColor;
            public string background { get; set; } = KendoConsts.DefaultBackground;
            public string textAlign { get; set; } = KendoConsts.DefaultTextAlign;
            public string verticalAlign { get; set; } = KendoConsts.DefaultVAlign;
            public bool bold { get; set; } = KendoConsts.DefaultBold;
            public bool wrap { get; set; } = KendoConsts.DefaultWrap;
            public double fontSize { get; set; } = KendoConsts.DefaultFontSize;
            public bool enable { get; set; } = KendoConsts.DefaultEnable;
            public string formula { get; set; }
            public bool underline { get; set; } = false;
            public bool italic { get; set; } = false;
            public string comment { get; set; }
        }

        public class SpreadSheet_Cell_Header : SpreadSheet_Cell
        {
            public int width { get; set; } = KendoConsts.HeaderWidth;
        }

        public class SpreadSheet_Column
        {
            public int index { get; set; }
            public double width { get; set; }
        }

        #endregion

        #region Chart

        public class ChartDto
        {
            public ChartTitle Title { get; set; }
            public ChartSeries[] Series { get; set; }
            public ChartValueAxis[] ValueAxis { get; set; }
            public ChartCategoryAxis CategoryAxis { get; set; }

            public ChartAxes[] ValueAxes { get; set; }

            public ChartLegend Legend { get; set; }
        }

        public class ChartTitle
        {
            public string Text { get; set; }
            public string Color { get; set; } = "#000000";
            public string Font { get; set; } = "14px sans-serif";
            public string Align { get; set; } = "center";
            public string Position { get; set; } = "top";
        }

        public class ChartLegend
        {
            public string Position { get; set; } = "top";
        }

        public class ChartSeries
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public string Axis { get; set; } = "default";

            public ChartLabelFormat Labels { get; set; }
            public decimal[] Data { get; set; }
        }

        public class ChartCategoryAxis
        {
            public string[] Categories { get; set; }

            public int[] AxisCrossingValues { get; set; }

            public RotateLabel Labels { get; set; } = new RotateLabel();
        }

        public class RotateLabel
        {
            public int Rotation { get; set; } = -45;
        }

        public class ChartValueAxis : ChartAxis
        {
        }

        public class ChartXAxis : ChartAxis
        {
        }

        public class ChartYAxis : ChartAxis
        {
        }

        public class ChartAxes
        {
            public string Name { get; set; } = "default";
            public ChartLabelFormat Labels { get; set; } = new ChartLabelFormat() {Format = "{0:n0}"};
        }

        public class ChartAxis
        {
            public string Name { get; set; } = "default";
            public ChartLabelFormat Labels { get; set; }
        }

        public class ChartLabelFormat
        {
            public bool Visible { get; set; } = true;
            public string Format { get; set; } = "{0}";
        }

        #endregion

        #region ChartPie

        public class ChartPieDto
        {
            public ChartTitle Title { get; set; }
            public ChartSeriesPie[] Series { get; set; }
        }

        public class ChartSeriesPie
        {
            public string Name { get; set; }
            public ChartLabelPieFormat Labels { get; set; }
            public CharPieValue[] Data { get; set; }
        }

        public class CharPieValue
        {
            public string Category { get; set; }
            public decimal Value { get; set; }
        }

        public class ChartLabelPieFormat
        {
            public bool Visible { get; set; } = false;
            public string Background { get; set; } = "transparent";
            public string Position { get; set; } = "center";
            public string Template { get; set; } = "#= value#%";
        }

        #endregion

        #region ImportModel

        public class GeneralImportDto
        {
            public string Name { get; set; }
            public List<SpreadSheet> Sheets { get; set; }
            public int ColCount { get; set; }
            public int RowCount { get; set; }
        }

        public class GeneralImportModel
        {
            public string ImportName { get; set; }

            /// <summary>
            /// Extend rows. Real rows = 50 + RowCount
            /// </summary>
            public int RowCount { get; set; }

            /// <summary>
            /// Extend input height. Real height = 25 + InputHeight
            /// </summary>
            public int InputHeight { get; set; }

            public bool HeaderCode { get; set; }

            /// <summary>
            /// Extend height. Real height = 50 + HeaderHeight
            /// </summary>
            public int HeaderHeight { get; set; }

            /// <summary>
            /// Extend height. Real height = 25 + HeaderCodeHeight
            /// </summary>
            public int HeaderCodeHeight { get; set; }

            public List<GeneralImportField> ListField { get; set; }
        }

        public class GeneralImportField
        {
            public string Header { get; set; }
            public string HeaderCode { get; set; }
            public int DataType { get; set; } = (int) ZeroEnums.DataType.Default;

            public int FractionalLetterCount { get; set; }

            /// <summary>
            /// Extend width. Real width = 100 + Width
            /// </summary>
            public int Width { get; set; }
        }

        #endregion
    }
}