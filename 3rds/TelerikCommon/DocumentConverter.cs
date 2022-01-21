using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Extensibility;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;

namespace TelerikCommon
{
    public class DocumentConverter
    {
        private readonly List<IFormatProvider<RadFlowDocument>> _providers;

        private RadFlowDocument _document;

        public DocumentConverter()
        {
            _providers = new List<IFormatProvider<RadFlowDocument>>()
            {
                new DocxFormatProvider(),
                new HtmlFormatProvider(),
                new PdfFormatProvider(),
                new RtfFormatProvider(),
                new TxtFormatProvider()
            };
            
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;
        }

        public void ConvertToPdf(Stream fs, string fileName, string savePath)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new Exception("FileName is null");
            if (string.IsNullOrEmpty(savePath))
                throw new Exception("SavePath is null");
            
            var extension = Path.GetExtension(fileName);
            var provider = _providers.FirstOrDefault(p => p.SupportedExtensions.Any(e => string.Compare(extension, e, StringComparison.InvariantCultureIgnoreCase) == 0));

            if (provider != null)
            {
                try
                {
                    JpegImageConverterBase customJpegImageConverter = new CustomJpegImageConverter(); 
                    FixedExtensibilityManager.JpegImageConverter = customJpegImageConverter; 
                    
                    _document = provider.Import(fs);
                    IFormatProvider<RadFlowDocument> formatProvider = new PdfFormatProvider();
                    // var path = FroalaEditor.File.GetAbsoluteServerPath(savePath);
                    // using (var stream = File.OpenWrite(path))
                    // {
                    //     formatProvider.Export(_document, stream);
                    // }
                    // var psi = new ProcessStartInfo()
                    // {
                    //     FileName = path,
                    //     UseShellExecute = true
                    // };
                    // Process.Start(psi);
                }
                catch (Exception)
                {
                    throw new Exception("Could not open file.");
                }
            }
            else
            {
                throw new Exception("Could not open file.");
            }
        }

        public void WordToPdf(byte[] data, string savePath)
        {
            if (string.IsNullOrEmpty(savePath))
                throw new Exception("SavePath is null");
            
            var provider = new DocxFormatProvider();
            try
            {
                _document = provider.Import(data);
                IFormatProvider<RadFlowDocument> formatProvider = new PdfFormatProvider();
                // var path = FroalaEditor.File.GetAbsoluteServerPath(savePath);
                // using (var stream = File.OpenWrite(path))
                // {
                //     formatProvider.Export(_document, stream);
                // }
            }
            catch (Exception)
            {
                throw new Exception("Could not open file.");
            }
            
        }
    }

    public class DocumentReplacer
    {
        private RadFlowDocument _document;
        private readonly List<IFormatProvider<RadFlowDocument>> _providers;
        public DocumentReplacer()
        {
            _providers = new List<IFormatProvider<RadFlowDocument>>()
            {
                new DocxFormatProvider(),
                new HtmlFormatProvider(),
                new PdfFormatProvider(),
                new RtfFormatProvider(),
                new TxtFormatProvider()
            };
            
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;
        }
        
        public void ReplaceAndConvertToPdf(string templateFileFullPath, Dictionary<string,string> replaceList, string savePath)
        {
            if (replaceList == null || !replaceList.Any())
                throw new Exception("ReplaceList is null");
            if (string.IsNullOrEmpty(templateFileFullPath))
                throw new Exception("TemplateFileName is null");
            if (string.IsNullOrEmpty(savePath))
                throw new Exception("SavePath is null");
            
            var extension = Path.GetExtension(templateFileFullPath);
            var provider = _providers.FirstOrDefault(p => p.SupportedExtensions.Any(e => string.Compare(extension, e, StringComparison.InvariantCultureIgnoreCase) == 0));

            if (provider != null)
            {
                try
                {
                    using (var fs = new FileStream(templateFileFullPath, FileMode.Open))
                    {
                        JpegImageConverterBase customJpegImageConverter = new CustomJpegImageConverter(); 
                        FixedExtensibilityManager.JpegImageConverter = customJpegImageConverter;
                        _document = provider.Import(fs);
                        var editor = new RadFlowDocumentEditor(_document);
                        foreach (var replace in replaceList)
                        {
                            editor.ReplaceText(replace.Key,replace.Value);
                        }
                        var formatProvider = new PdfFormatProvider();
                        using (var stream = File.OpenWrite(savePath))
                        {
                            formatProvider.Export(_document, stream);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Could not open file.");
                }
            }
            else
            {
                throw new Exception("Could not open file.");
            }
        }
    }
}