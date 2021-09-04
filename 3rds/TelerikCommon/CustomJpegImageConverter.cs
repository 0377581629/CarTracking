using System;
using System.Linq;
using ImageMagick;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;

namespace TelerikCommon
{
    internal class CustomJpegImageConverter : Telerik.Windows.Documents.Extensibility.JpegImageConverterBase 
    { 
        public override bool TryConvertToJpegImageData(byte[] imageData, ImageQuality imageQuality, out byte[] jpegImageData) 
        { 
            string[] magickImageFormats = Enum.GetNames(typeof(MagickFormat)).Select(x => x.ToLower()).ToArray(); 
            string imageFormat; 
            if (this.TryGetImageFormat(imageData, out imageFormat) && magickImageFormats.Contains(imageFormat.ToLower())) 
            { 
                using (MagickImage magickImage = new MagickImage(imageData)) 
                { 
                    magickImage.Format = MagickFormat.Jpeg; 
                    magickImage.Alpha(AlphaOption.Remove); 
                    magickImage.Quality = (int)imageQuality; 
 
                    jpegImageData = magickImage.ToByteArray(); 
                } 
 
                return true; 
            } 
 
            jpegImageData = null; 
            return false; 
        } 
    } 
}