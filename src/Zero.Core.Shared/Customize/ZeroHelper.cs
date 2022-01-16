using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using shortid;
using shortid.Configuration;
using Zero.Extensions;

namespace Zero.Customize
{
    public static class StatusHelper
    {
        public static bool AllowEditDetailStatus(int status)
        {
            return status == 0 || status == (int) ZeroEnums.DefaultStatus.Draft || status == (int) ZeroEnums.DefaultStatus.Return;
        }
        public static string StatusContent(int status, ILocalizationSource lang)
        {
            var obj = (ZeroEnums.DefaultStatus) status;
            return lang.GetString(obj.GetStringValue());
        }
        
        public static List<SelectListItem> ListStatus(int currentStatus, ILocalizationSource lang)
        {
            return (from status in (ZeroEnums.DefaultStatus[]) Enum.GetValues(typeof(ZeroEnums.DefaultStatus)) select new SelectListItem(lang.GetString(status.GetStringValue()), ((int) status).ToString(), currentStatus == (int) status)).ToList();
        }
    }
    
    public static class DateTimeHelper
    {
        public static void QuarterRange(int quarter, int year, ref DateTime startDate, ref DateTime endDate)
        {
            var targetDate = new DateTime(year,quarter*3,1);
            QuarterRange(targetDate, ref startDate, ref endDate);
        }

        private static void QuarterRange(DateTime input, ref DateTime startDate, ref DateTime endDate)
        {
            var inputMonth = input.Month;
            var inputYear = input.Year;

            if (inputMonth.IsBetween(1, 3))
            {
                startDate = new DateTime(inputYear, 1, 1);
                endDate = startDate.AddMonths(3).AddMinutes(-1);
            }

            if (inputMonth.IsBetween(4, 6))
            {
                startDate = new DateTime(inputYear, 4, 1);
                endDate = startDate.AddMonths(3).AddMinutes(-1);
            }

            if (inputMonth.IsBetween(7, 9))
            {
                startDate = new DateTime(inputYear, 7, 1);
                endDate = startDate.AddMonths(3).AddMinutes(-1);
            }

            if (inputMonth.IsBetween(10, 12))
            {
                startDate = new DateTime(inputYear, 10, 1);
                endDate = startDate.AddMonths(3).AddMinutes(-1);
            }
        }

        public static void HaftYearRange(DateTime input, ref DateTime startDate, ref DateTime endDate)
        {
            var inputMonth = input.Month;
            var inputYear = input.Year;

            if (inputMonth.IsBetween(1, 6))
            {
                startDate = new DateTime(inputYear, 1, 1);
                endDate = startDate.AddMonths(6).AddMinutes(-1);
            }

            if (inputMonth.IsBetween(7, 12))
            {
                startDate = new DateTime(inputYear, 7, 1);
                endDate = startDate.AddMonths(6).AddMinutes(-1);
            }
        }

        public static void YearRange(DateTime input, ref DateTime startDate, ref DateTime endDate)
        {
            var inputYear = input.Year;
            startDate = new DateTime(inputYear, 1, 1);
            endDate = startDate.AddYears(1).AddMinutes(-1);
        }

        public static List<SelectListItem> ListMonth(int selected = 0)
        {
            return new()
            {
                new SelectListItem("1", "1", selected == 1),
                new SelectListItem("2", "2", selected == 2),
                new SelectListItem("3", "3", selected == 3),
                new SelectListItem("4", "4", selected == 4),
                new SelectListItem("5", "5", selected == 5),
                new SelectListItem("6", "6", selected == 6),
                new SelectListItem("7", "7", selected == 7),
                new SelectListItem("8", "8", selected == 8),
                new SelectListItem("9", "9", selected == 9),
                new SelectListItem("10", "10", selected == 10),
                new SelectListItem("11", "11", selected == 11),
                new SelectListItem("12", "12", selected == 12),
            };
        }

        public static List<SelectListItem> ListQuarter(int selected = 0)
        {
            return new()
            {
                new SelectListItem("Quý I", "1", selected == 1),
                new SelectListItem("Quý II", "2", selected == 2),
                new SelectListItem("Quý III", "3", selected == 3),
                new SelectListItem("Quý IV", "4", selected == 4)
            };
        }
        
        public static List<SelectListItem> ListYear(int selected = 0)
        {
            {
                var res = new List<SelectListItem>();
                for (var i = 2000; i < 2100; i++)
                {
                    res.Add(new SelectListItem(i.ToString(), i.ToString(), selected == i));
                }

                return res;
            }
        }
        
        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dateTime;
        }
    }

    public static class StringHelper
    {
        public static string RemoveVietnameseTone(string text)
        {
            var result = RemoveSymbol(text.ToLower());
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }
        
        public static string RemoveSymbol(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]", string.Empty);;
        }
        
        public static string Identity()
        {
            return "A" + Guid.NewGuid().ToString().Replace("-","").ToUpper();
        }
        
        public static string ShortIdentity(int length=8, bool useSpecialChar = false, bool useNumber = true)
        {
            if (length < 8)
                length = 8;
            return ShortId.Generate(new GenerationOptions
            {
                Length = length,
                UseSpecialCharacters = false,
                UseNumbers = true
            }).ToUpper();
        }
        
        public static string CodeFormat(string formatInput, string suffixCode)
        {
            var res = formatInput;
            if (suffixCode.Length > res.Length)
                return suffixCode;
            var prefix = res.Substring(0, res.Length - suffixCode.Length);
            res = prefix + suffixCode;
            return res;
        }
        
        public static string ShortCodeFromString(string input)
        {
            var res = "";
            if (!string.IsNullOrEmpty(input))
                Array.ForEach(input.Split(" ", StringSplitOptions.RemoveEmptyEntries), s => res += s[0]);
            return new string(res.Where(char.IsLetter).ToArray()).ToUpper();
        }

        public static string UpperFirstLetterOfWord(string input)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static string GetFirstLetterOfWord(string input)
        {
            try
            {
                var res = "";
                if (!string.IsNullOrEmpty(input))
                    Array.ForEach(input.Trim().Split(' '), s => res += s[0]);
                return res;
            }
            catch (Exception)
            {
                return "";
            }
        }
        
        public static string UpperFirstChar(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var res = input.ToLower();
            res = res[0].ToString().ToUpper() + res.Substring(1, res.Length - 1);
            return res;
        }

        public static string CharByIndex(int index)
        {
            char[] arrayTitle = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};
            return arrayTitle[index].ToString();
        }
        
        public static string NewLineByWord(string input, string seperator, int rangeSize)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return input;
            var words = input.Split(' ').ToList();
            if (words.Count <= rangeSize) return input;

            var tempWords = new List<string>();

            while (words.Count > rangeSize)
            {
                var rangeWord = "";
                for (var i = 0; i < rangeSize; i++)
                {
                    rangeWord += words[i] + " ";
                }

                tempWords.Add(rangeWord);
                words.RemoveRange(0, rangeSize);
            }

            return string.Join(seperator, tempWords);
        }
        
        public static string ShortTextByWord(string input, int rangeSize=15)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return input;
            var words = input.Split(' ').ToList();
            if (words.Count <= rangeSize) return input;

            var tempWords = new List<string>();

            while (words.Count > rangeSize)
            {
                var rangeWord = "";
                for (var i = 0; i < rangeSize; i++)
                {
                    rangeWord += words[i] + " ";
                }

                tempWords.Add(rangeWord);
                words.RemoveRange(0, rangeSize);
            }

            if (tempWords.Count > 1)
                return tempWords.First() + " ...";
            return tempWords.First();
        }

        public static string Tab(int count)
        {
            var res = "";
            for (var i = 0; i < count; i++)
            {
                res += "    ";
            }

            return res;
        }
    }

    public static class FileHelper
    {
        private const string ContentFolderRoot = "Files";
        
        public static string FileServerRootPath(IMultiTenancyConfig multiTenancyConfig, IAbpSession abpSession, bool isAdminUser)
        {
            var rootPath = SystemConfig.MinioRootBucketName;

            if (!multiTenancyConfig.IsEnabled) 
                return rootPath;

            if (abpSession.MultiTenancySide == MultiTenancySides.Host && isAdminUser)
                return rootPath;
            
            rootPath = Path.Combine(rootPath, abpSession.TenantId.HasValue ? abpSession.TenantId.ToString() : "Host");
            
            if (!isAdminUser)
            {
                rootPath = Path.Combine(rootPath, abpSession.UserId.ToString());
            }

            return rootPath;
        }
        
        public static string UploadPath(IMultiTenancyConfig multiTenancyConfig, IAbpSession abpSession, bool isAdminUser, ZeroEnums.FileType type = ZeroEnums.FileType.Root)
        {
            var uploadPath = Path.Combine($"{Path.DirectorySeparatorChar}{ContentFolderRoot}");
            if (!multiTenancyConfig.IsEnabled)
            {
                if (!isAdminUser && abpSession.UserId.HasValue)
                    uploadPath = Path.Combine(uploadPath, abpSession.UserId.Value.ToString());
                if (type != ZeroEnums.FileType.Root)
                    uploadPath = Path.Combine(uploadPath, TypePath(type));
                return uploadPath;
            }
            
            uploadPath = Path.Combine(uploadPath, abpSession.TenantId.HasValue ? abpSession.TenantId.ToString() : "Host");
            if (!isAdminUser && abpSession.UserId.HasValue)
                uploadPath = Path.Combine(uploadPath, abpSession.UserId.Value.ToString());
            if (type != ZeroEnums.FileType.Root)
                uploadPath = Path.Combine(uploadPath, TypePath(type));
            return uploadPath;
        }

        private static string TypePath(ZeroEnums.FileType type)
        {
            return type switch
            {
                ZeroEnums.FileType.Root => "",
                ZeroEnums.FileType.Image => "Image",
                ZeroEnums.FileType.Audio => "Audio",
                ZeroEnums.FileType.Video => "Video",
                ZeroEnums.FileType.Office => "Office",
                ZeroEnums.FileType.Compress => "Compress",
                _ => "Others"
            };
        }
        
        private static List<string> _knownTypes;

        public static void Validate(IFormFile uploadingFile)
        {
            if (!IsSafe(uploadingFile.ContentType)) 
                throw new Exception("InvalidFile");
        }
        
        private static bool IsSafe(string fileMimeType)
        {
            if (_knownTypes == null)
                InitializeMimeTypeLists();
            return _knownTypes != null && _knownTypes.Contains(fileMimeType);
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)] 
            byte[] pBuffer,
            int cbSize,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
            int dwMimeFlags,
            out IntPtr ppwzMimeOut,
            int dwReserved);

        private static string ScanFileForMimeTypeStream(Stream fs)
        {
            try
            {
                var buffer = new byte[256];

                var readLength = Convert.ToInt32(Math.Min(256, fs.Length));
                fs.Read(buffer, 0, readLength);

                IntPtr outPtr;
                var mime = "";
                var ret = FindMimeFromData(IntPtr.Zero, null, buffer, 256, null, 0, out outPtr, 0);

                if (ret == 0 && outPtr != IntPtr.Zero)
                {
                    //todo: this leaks memory outPtr must be freed
                    mime = Marshal.PtrToStringUni(outPtr);
                }

                //var mimeTypePtr = new IntPtr(mimeType);
                //var mime = Marshal.PtrToStringUni(mimeTypePtr);
                //Marshal.FreeCoTaskMem(mimeTypePtr);
                if (string.IsNullOrEmpty(mime))
                    mime = "application/octet-stream";
                return mime;
            }
            catch (Exception)
            {
                return "application/octet-stream";
            }
            finally
            {
                fs.Close();
            }
        }
        
        private static void InitializeMimeTypeLists()
        {
            
            _knownTypes = new List<string>
            {
                // Image
                "image/gif", // .gif
                "image/vnd.microsoft.icon", //.ico
                "image/jpeg", // .jpeg .jpg
                "image/pjpeg",
                
                "image/png", // .png
                "image/x-png",
                
                "image/bmp", // .bmp
                "image/svg+xml",
                
                // Audio
                "audio/mpeg", // .mp3
                
                // Video
                "video/mp4",
                "video/webm",
                "video/ogg",

                // MS Word
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                // MS PP
                "application/vnd.ms-powerpoint",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                // MS Excel
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                
                // Open document
                "application/vnd.oasis.opendocument.presentation",
                "application/vnd.oasis.opendocument.spreadsheet",
                "application/vnd.oasis.opendocument.text",
                
                // Compress
                "application/zip", // .zip
                "application/vnd.rar", // .rar
                "application/gzip", // .gz
                
                // Other
                "text/plain", // .txt
                "text/html", // .html
                "text/xml", // .xml
                "application/rtf", // .rtf
            };
        }
    }

    public static class EmailHelper
    {
        public static string EmailTemplateType(int emailTemplateType, ILocalizationSource lang)
        {
            var type = (ZeroEnums.EmailTemplateType) emailTemplateType;
            return lang.GetString(type.GetStringValue());
        }

        public static List<SelectListItem> ListEmailTemplateType(int currentType, ILocalizationSource lang)
        {
            return (from emailTemplateType in (ZeroEnums.EmailTemplateType[]) Enum.GetValues(typeof(ZeroEnums.EmailTemplateType))
                select new SelectListItem(lang.GetString(emailTemplateType.GetStringValue()), ((int) emailTemplateType).ToString(), currentType == (int) emailTemplateType)).ToList();
        }
    }

    public static class SelectListHelper
    {
        public static List<SelectListItem> ListWithNull(string nullContent, List<SelectListItem> lstSource, long? current=null)
        {
            var res = new List<SelectListItem>
            {
                new(nullContent, "")
            };
            
            if (lstSource != null && lstSource.Any())
                res.AddRange(lstSource);
            if (!current.HasValue || lstSource == null) return res;

            foreach (var itm in lstSource.Where(itm => !string.IsNullOrEmpty(itm.Value)))
            {
                itm.Selected = Convert.ToInt64(itm.Value) == current.Value;
            }

            return res;
        }
        
        public static List<SelectListItem> ListWithNone(string noneContent, List<SelectListItem> lstSource)
        {
            var res = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(noneContent))
            {
                res.Insert(0, new SelectListItem(noneContent, "0"));
            }
            
            if (lstSource != null && lstSource.Any())
                res.AddRange(lstSource);
            
            return res;
        }

        public static List<SelectListItem> ListWithNone(string noneContent, List<SelectListItem> lstSource, long? selected)
        {
            var res = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(noneContent))
            {
                res.Insert(0, new SelectListItem(noneContent, "0"));
            }
            if (lstSource != null && lstSource.Any())
                res.AddRange(lstSource);
            foreach (var item in res)
            {
                item.Selected = item.Value == selected.ToString();
            }
            return res;
        }
        
        public static List<SelectListItem> ListWithNoData(string noneContent, List<SelectListItem> lstSource)
        {
            if (lstSource != null && lstSource.Any())
                return lstSource;
            
            return new List<SelectListItem>
            {
                new(noneContent, "0")
            };
        }

        public static List<SelectListItem> ListWithNoData(string noneContent, List<SelectListItem> lstSource,
            long selected)
        {
            if (lstSource != null && lstSource.Any())
            {
                if (selected.ToString() != "0")
                {
                    foreach (var item in lstSource.Where(item => item.Value == selected.ToString()))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                return lstSource;
            }

            return new List<SelectListItem>
            {
                new(noneContent, "0")
            };
        }
    }

    public static class ImportExcelHelper
    {
        public static string GetValueFormCell(ICell cell)
        {
            var data = string.Empty;
            if (cell == null) return data;

            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Formula:
                    try
                    {
                        return cell.StringCellValue;
                    }
                    catch (Exception)
                    {
                        return cell.NumericCellValue.ToString(ZeroConst.NumberFormatInfo);
                    }
                case CellType.Numeric:
                    data = DateUtil.IsCellDateFormatted(cell)
                        ? cell.DateCellValue.ToString(ZeroConst.DateFormat)
                        : cell.NumericCellValue.ToString(ZeroConst.NumberFormatInfo);
                    break;
                case CellType.String:
                    data = cell.StringCellValue;
                    break;
                case CellType.Unknown:
                    break;
                case CellType.Blank:
                    break;
                default:
                    return "";
            }

            return data;
        }

        public static string GetName<T>(string displayName)
        {
            var listPi = typeof(T).GetProperties();
            var name = string.Empty;

            foreach (var pi in listPi)
            {
                var dp = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>()
                    .SingleOrDefault();
                if (dp != null && displayName == dp.DisplayName)
                {
                    name = pi.Name;
                }
            }

            return name;
        }

        public static string TranslateColumnIndexToName(int index)
        {
            var quotient = (index) / 26;

            if (quotient > 0)
            {
                return TranslateColumnIndexToName(quotient - 1) + (char) ((index % 26) + 65);
            }
            else
            {
                return "" + (char) ((index % 26) + 65);
            }
        }

        public static string GetRequiredValueFromRowOrNull(ISheet worksheet, int row, int column, string columnName,
            StringBuilder exceptionMessage, ILocalizationSource localizationSource)
        {
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName, localizationSource));
            return null;
        }

        private static string GetLocalizedExceptionMessagePart(string parameter, ILocalizationSource localizationSource)
        {
            return localizationSource.GetString("{0}IsInvalid", localizationSource.GetString(parameter)) + "; ";
        }

        public static bool IsRowEmpty(ISheet worksheet, int row)
        {
            var cell = worksheet.GetRow(row)?.Cells.FirstOrDefault();
            return cell == null || string.IsNullOrWhiteSpace(cell.StringCellValue);
        }
    }

    public static class NestedItemHelper
    {
        public static void BuildRecursiveItem(ref List<NestedItem.NestedItem> lstSource)
        {
            // List parent
            var lstParent = lstSource.Where(o => o.ParentId == null).ToList();

            foreach (var item in lstParent)
            {
                AddChildItem(lstSource, item);
            }

            lstSource = lstParent;
        }
        
        private static void AddChildItem(IEnumerable<NestedItem.NestedItem> lstSource, NestedItem.NestedItem item)
        {
            var enumerable = lstSource.ToList();
            var childItems = enumerable.Where(o => o.ParentId == item.Id && o.Id > 0).ToList();
            if (childItems.Count <= 0) return;
            foreach (var cItem in childItems)
            {
                item.Children ??= new List<NestedItem.NestedItem>();
                AddChildItem(enumerable, cItem);
                item.Children.Add(cItem);
            }
        }
    }
}