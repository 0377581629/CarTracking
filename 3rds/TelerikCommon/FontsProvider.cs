using System;
using System.IO;
using Telerik.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Extensibility;

namespace TelerikCommon
{
    internal class FontsProvider : FontsProviderBase
    {
        private readonly string _fontFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

        public override byte[] GetFontData(FontProperties fontProperties)
        {
            var fontFamilyName = fontProperties.FontFamilyName;
            var isBold = fontProperties.FontWeight == FontWeights.Bold;
            var fontFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            if (fontFamilyName == "Arial" && isBold)
            {
                using (var fileStream = File.OpenRead(fontFolder + "\\arialbd.ttf"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            else if (fontFamilyName == "Arial")
            {
                return GetFontDataFromFontFolder("arial.ttf");
            }
            else if (fontFamilyName == "Calibri" && isBold)
            {
                return GetFontDataFromFontFolder("calibrib.ttf");
            }
            else if (fontFamilyName == "Calibri")
            {
                return GetFontDataFromFontFolder("calibri.ttf");
            }

            return null;
        }

        private byte[] GetFontDataFromFontFolder(string fontFileName)
        {
            using (var fileStream = File.OpenRead(_fontFolder + "\\" + fontFileName))
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}