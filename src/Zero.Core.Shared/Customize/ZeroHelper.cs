using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Extensions;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using shortid;
using shortid.Configuration;
using Zero.Customize.NestedItem;
using Zero.Extensions;

namespace Zero
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
        public static string AgeRange(int ageRange, ILocalizationSource lang)
        {
            var obj = (ZeroEnums.DefaultAgeRange) ageRange;
            return lang.GetString(obj.GetStringValue());
        }

        public static void AgeRangeToYearRange(int ageRange, out int startYear, out int endYear)
        {
            switch ((ZeroEnums.DefaultAgeRange)ageRange)
            {
                case ZeroEnums.DefaultAgeRange.Small:
                    startYear = DateTime.Today.Year - 19;
                    endYear = DateTime.Today.Year;
                    break;
                case ZeroEnums.DefaultAgeRange.Young:
                    startYear = DateTime.Today.Year - 25;
                    endYear = DateTime.Today.Year - 18;
                    break;
                case ZeroEnums.DefaultAgeRange.Middle:
                    startYear = DateTime.Today.Year - 45;
                    endYear = DateTime.Today.Year - 26;
                    break;
                case ZeroEnums.DefaultAgeRange.Old:
                    startYear = 1900;
                    endYear = DateTime.Today.Year - 46;
                    break;
                default:
                    startYear = 1900;
                    endYear = DateTime.Today.Year;
                    break;
            }
        }
        
        public static List<SelectListItem> ListAgeRange(int? current, ILocalizationSource lang)
        {
            return (from ageRange in (ZeroEnums.DefaultAgeRange[]) Enum.GetValues(typeof(ZeroEnums.DefaultAgeRange)) select new SelectListItem(lang.GetString(ageRange.GetStringValue()), ((int) ageRange).ToString(), current == (int) ageRange)).ToList();
        }
        
        public static void QuarterRange(int quarter, int year, ref DateTime startDate, ref DateTime endDate)
        {
            var targetDate = new DateTime(year,quarter*3,1);
            QuarterRange(targetDate, ref startDate, ref endDate);
        }
        public static void QuarterRange(DateTime input, ref DateTime startDate, ref DateTime endDate)
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
            var inputMonth = input.Month;
            var inputDay = input.Day;
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
            return ShortId.Generate(new GenerationOptions()
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
        public static string UploadPath(IAbpSession abpSession, ZeroEnums.FileType type)
        {
            var userPath = "";
            if (abpSession.TenantId.HasValue)
                userPath += "Tenant" + abpSession.TenantId + "/";
            else
                userPath += "Host/";

            if (abpSession.UserId.HasValue)
                userPath += abpSession.UserId + "/";

            return "wwwroot/uploads/" + userPath + TypePath(type) + "/";
        }

        public static bool CheckPermissionPath(IAbpSession abpSession,
            ZeroEnums.FileType type, string checkPath)
        {
            if (string.IsNullOrEmpty(checkPath)) return false;
            var path = UploadPath(abpSession, type);
            return checkPath.IndexOf(path, StringComparison.Ordinal) == 0;
        }

        private static string TypePath(ZeroEnums.FileType type)
        {
            switch (type)
            {
                case ZeroEnums.FileType.Image:
                    return "Image";
                case ZeroEnums.FileType.Audio:
                    return "Audio";
                case ZeroEnums.FileType.Video:
                    return "Video";
                case ZeroEnums.FileType.Office:
                    return "Office";
                case ZeroEnums.FileType.Compress:
                    return "Compress";
                default:
                    return "Others";
            }
        }
        
        private static List<string> _knownTypes;

        private static Dictionary<string, string> _mimeTypes;

        public static bool Validate(Stream fs, string fileName, ZeroEnums.FileTypeByApp target)
        {
            if (_knownTypes == null || _mimeTypes == null)
                InitializeMimeTypeLists();
            var contentType = "";
            var extension = Path.GetExtension(fileName).Remove(0, 1).ToLower();
            if (_mimeTypes != null) _mimeTypes.TryGetValue(extension, out contentType);
            var fileMimeType = ScanFileForMimeTypeStream(fs);

            if (IsSafe(fileMimeType))
            {
                switch (target)
                {
                    case ZeroEnums.FileTypeByApp.Word:
                        if (contentType == "application/msword" &&
                            (fileMimeType == "application/x-zip-compressed" ||
                             fileMimeType == "application/octet-stream"))
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.Excel:
                        if (contentType == "application/excel" &&
                            (fileMimeType == "application/x-zip-compressed" ||
                             fileMimeType == "application/octet-stream"))
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.PowerPoint:
                        if (contentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation" && fileMimeType == "application/octet-stream")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.Pdf:
                        if (contentType == "application/pdf" && fileMimeType == "application/pdf")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.RTF:
                        if (contentType == "text/richtext" && fileMimeType == "text/richtext")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.TXT:
                        if (contentType == "text/plain" && fileMimeType == "application/octet-stream")
                            return true;
                        break;
                }
            }
            return false;
        }

        public static bool AbleConvertToPdf(Stream fs, string fileName)
        {
            var lstTarget = new List<ZeroEnums.FileTypeByApp>()
            {
                ZeroEnums.FileTypeByApp.Word,
                ZeroEnums.FileTypeByApp.Pdf,
                ZeroEnums.FileTypeByApp.RTF,
                ZeroEnums.FileTypeByApp.TXT
            };
            return Validate(fs, fileName, lstTarget);
        }

        public static bool AbleConvertToPdf(string physPath)
        {
            var lstTarget = new List<ZeroEnums.FileTypeByApp>()
            {
                ZeroEnums.FileTypeByApp.Word,
                ZeroEnums.FileTypeByApp.Pdf,
                ZeroEnums.FileTypeByApp.RTF,
                ZeroEnums.FileTypeByApp.TXT
            };
            var fileName = Path.GetFileName(physPath);
            using var fs = new FileStream(physPath, FileMode.Open);
            return Validate(fs, fileName, lstTarget);
        }
        
        private static bool Validate(Stream fs, string fileName, List<ZeroEnums.FileTypeByApp> lstTarget)
        {
            if (lstTarget == null || !lstTarget.Any())
                return false;
            if (_knownTypes == null || _mimeTypes == null)
                InitializeMimeTypeLists();
            var contentType = "";
            var extension = Path.GetExtension(fileName).Remove(0, 1).ToLower();
            _mimeTypes?.TryGetValue(extension, out contentType);
            var fileMimeType = ScanFileForMimeTypeStream(fs);

            if (!IsSafe(fileMimeType)) return false;
            
            foreach (var target in lstTarget)
            {
                switch (target)
                {
                    case ZeroEnums.FileTypeByApp.Word:
                        if (contentType == "application/msword" &&
                            (fileMimeType == "application/x-zip-compressed" ||
                             fileMimeType == "application/octet-stream"))
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.Excel:
                        if (contentType == "application/excel" &&
                            (fileMimeType == "application/x-zip-compressed" ||
                             fileMimeType == "application/octet-stream"))
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.PowerPoint:
                        if (contentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation" && fileMimeType == "application/octet-stream")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.Pdf:
                        if (contentType == "application/pdf" && fileMimeType == "application/pdf")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.RTF:
                        if (contentType == "text/richtext" && fileMimeType == "text/richtext")
                            return true;
                        break;
                    case ZeroEnums.FileTypeByApp.TXT:
                        if (contentType == "text/plain" && fileMimeType == "application/octet-stream")
                            return true;
                        break;
                }
            }
            return false;
        }
        
        private static bool IsSafe(string fileMimeType)
        {
            if (_knownTypes == null || _mimeTypes == null)
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
        private static string ScanFileForMimeType(string fileName)
        {
            try
            {
                // var buffer = new byte[256];
                // using (var fs = new FileStream(fileName, FileMode.Open))
                // {
                //     var readLength = Convert.ToInt32(Math.Min(256, fs.Length));
                //     fs.Read(buffer, 0, readLength);
                // }
                //
                // var mimeType = default(UInt32);
                // FindMimeFromData(IntPtr.Zero, null, buffer, 256, null, 0, ref mimeType, 0);
                // var mimeTypePtr = new IntPtr(mimeType);
                // var mime = Marshal.PtrToStringUni(mimeTypePtr);
                // Marshal.FreeCoTaskMem(mimeTypePtr);
                // if (string.IsNullOrEmpty(mime))
                //     mime = "application/octet-stream";
                // return mime;
                return "";
            }
            catch (Exception)
            {
                return "application/octet-stream";
            }
        }
        private static void InitializeMimeTypeLists()
        {
            _knownTypes = new List<string>()
            {
                "text/plain",
                "text/html",
                "text/xml",
                "text/richtext",
                "text/scriptlet",
                "audio/x-aiff",
                "audio/basic",
                "audio/mid",
                "audio/wav",
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png",
                "image/x-png",
                "image/tiff",
                "image/bmp",
                "image/x-xbitmap",
                "image/x-jg",
                "image/x-emf",
                "image/x-wmf",
                "video/avi",
                "video/mpeg",
                "application/octet-stream",
                "application/postscript",
                "application/base64",
                "application/macbinhex40",
                "application/pdf",
                "application/xml",
                "application/atom+xml",
                "application/rss+xml",
                "application/x-compressed",
                "application/x-zip-compressed",
                "application/x-gzip-compressed",
                "application/java",
                "application/x-msdownload"
            };

            _mimeTypes = new Dictionary<string, string>();
            _mimeTypes.Add("3dm", "x-world/x-3dmf");
            _mimeTypes.Add("3dmf", "x-world/x-3dmf");
            _mimeTypes.Add("a", "application/octet-stream");
            _mimeTypes.Add("aab", "application/x-authorware-bin");
            _mimeTypes.Add("aam", "application/x-authorware-map");
            _mimeTypes.Add("aas", "application/x-authorware-seg");
            _mimeTypes.Add("abc", "text/vnd.abc");
            _mimeTypes.Add("acgi", "text/html");
            _mimeTypes.Add("afl", "video/animaflex");
            _mimeTypes.Add("ai", "application/postscript");
            _mimeTypes.Add("aif", "audio/aiff");
            _mimeTypes.Add("aifc", "audio/aiff");
            _mimeTypes.Add("aiff", "audio/aiff");
            _mimeTypes.Add("aim", "application/x-aim");
            _mimeTypes.Add("aip", "text/x-audiosoft-intra");
            _mimeTypes.Add("ani", "application/x-navi-animation");
            _mimeTypes.Add("aos", "application/x-nokia-9000-communicator-add-on-software");
            _mimeTypes.Add("aps", "application/mime");
            _mimeTypes.Add("arc", "application/octet-stream");
            _mimeTypes.Add("arj", "application/arj");
            _mimeTypes.Add("art", "image/x-jg");
            _mimeTypes.Add("asf", "video/x-ms-asf");
            _mimeTypes.Add("asm", "text/x-asm");
            _mimeTypes.Add("asp", "text/asp");
            _mimeTypes.Add("asx", "application/x-mplayer2");
            _mimeTypes.Add("au", "audio/basic");
            _mimeTypes.Add("avi", "video/avi");
            _mimeTypes.Add("avs", "video/avs-video");
            _mimeTypes.Add("bcpio", "application/x-bcpio");
            _mimeTypes.Add("bin", "application/octet-stream");
            _mimeTypes.Add("bm", "image/bmp");
            _mimeTypes.Add("bmp", "image/bmp");
            _mimeTypes.Add("boo", "application/book");
            _mimeTypes.Add("book", "application/book");
            _mimeTypes.Add("boz", "application/x-bzip2");
            _mimeTypes.Add("bsh", "application/x-bsh");
            _mimeTypes.Add("bz", "application/x-bzip");
            _mimeTypes.Add("bz2", "application/x-bzip2");
            _mimeTypes.Add("c", "text/plain");
            _mimeTypes.Add("c++", "text/plain");
            _mimeTypes.Add("cat", "application/vnd.ms-pki.seccat");
            _mimeTypes.Add("cc", "text/plain");
            _mimeTypes.Add("ccad", "application/clariscad");
            _mimeTypes.Add("cco", "application/x-cocoa");
            _mimeTypes.Add("cdf", "application/cdf");
            _mimeTypes.Add("cer", "application/pkix-cert");
            _mimeTypes.Add("cha", "application/x-chat");
            _mimeTypes.Add("chat", "application/x-chat");
            _mimeTypes.Add("class", "application/java");
            _mimeTypes.Add("com", "application/octet-stream");
            _mimeTypes.Add("conf", "text/plain");
            _mimeTypes.Add("cpio", "application/x-cpio");
            _mimeTypes.Add("cpp", "text/x-c");
            _mimeTypes.Add("cpt", "application/x-cpt");
            _mimeTypes.Add("crl", "application/pkcs-crl");
            _mimeTypes.Add("css", "text/css");
            _mimeTypes.Add("def", "text/plain");
            _mimeTypes.Add("der", "application/x-x509-ca-cert");
            _mimeTypes.Add("dif", "video/x-dv");
            _mimeTypes.Add("dir", "application/x-director");
            _mimeTypes.Add("dl", "video/dl");
            _mimeTypes.Add("doc", "application/msword");
            _mimeTypes.Add("docx", "application/msword");
            _mimeTypes.Add("dot", "application/msword");
            _mimeTypes.Add("dp", "application/commonground");
            _mimeTypes.Add("drw", "application/drafting");
            _mimeTypes.Add("dump", "application/octet-stream");
            _mimeTypes.Add("dv", "video/x-dv");
            _mimeTypes.Add("dvi", "application/x-dvi");
            _mimeTypes.Add("dwf", "drawing/x-dwf (old)");
            _mimeTypes.Add("dwg", "application/acad");
            _mimeTypes.Add("dxf", "application/dxf");
            _mimeTypes.Add("eps", "application/postscript");
            _mimeTypes.Add("es", "application/x-esrehber");
            _mimeTypes.Add("etx", "text/x-setext");
            _mimeTypes.Add("evy", "application/envoy");
            _mimeTypes.Add("exe", "application/octet-stream");
            _mimeTypes.Add("f", "text/plain");
            _mimeTypes.Add("f90", "text/x-fortran");
            _mimeTypes.Add("fdf", "application/vnd.fdf");
            _mimeTypes.Add("fif", "image/fif");
            _mimeTypes.Add("fli", "video/fli");
            _mimeTypes.Add("flv", "video/x-flv");
            _mimeTypes.Add("for", "text/x-fortran");
            _mimeTypes.Add("fpx", "image/vnd.fpx");
            _mimeTypes.Add("g", "text/plain");
            _mimeTypes.Add("g3", "image/g3fax");
            _mimeTypes.Add("gif", "image/gif");
            _mimeTypes.Add("gl", "video/gl");
            _mimeTypes.Add("gsd", "audio/x-gsm");
            _mimeTypes.Add("gtar", "application/x-gtar");
            _mimeTypes.Add("gz", "application/x-compressed");
            _mimeTypes.Add("h", "text/plain");
            _mimeTypes.Add("help", "application/x-helpfile");
            _mimeTypes.Add("hgl", "application/vnd.hp-hpgl");
            _mimeTypes.Add("hh", "text/plain");
            _mimeTypes.Add("hlp", "application/x-winhelp");
            _mimeTypes.Add("htc", "text/x-component");
            _mimeTypes.Add("htm", "text/html");
            _mimeTypes.Add("html", "text/html");
            _mimeTypes.Add("htmls", "text/html");
            _mimeTypes.Add("htt", "text/webviewhtml");
            _mimeTypes.Add("htx", "text/html");
            _mimeTypes.Add("ice", "x-conference/x-cooltalk");
            _mimeTypes.Add("ico", "image/x-icon");
            _mimeTypes.Add("idc", "text/plain");
            _mimeTypes.Add("ief", "image/ief");
            _mimeTypes.Add("iefs", "image/ief");
            _mimeTypes.Add("iges", "application/iges");
            _mimeTypes.Add("igs", "application/iges");
            _mimeTypes.Add("ima", "application/x-ima");
            _mimeTypes.Add("imap", "application/x-httpd-imap");
            _mimeTypes.Add("inf", "application/inf");
            _mimeTypes.Add("ins", "application/x-internett-signup");
            _mimeTypes.Add("ip", "application/x-ip2");
            _mimeTypes.Add("isu", "video/x-isvideo");
            _mimeTypes.Add("it", "audio/it");
            _mimeTypes.Add("iv", "application/x-inventor");
            _mimeTypes.Add("ivr", "i-world/i-vrml");
            _mimeTypes.Add("ivy", "application/x-livescreen");
            _mimeTypes.Add("jam", "audio/x-jam");
            _mimeTypes.Add("jav", "text/plain");
            _mimeTypes.Add("java", "text/plain");
            _mimeTypes.Add("jcm", "application/x-java-commerce");
            _mimeTypes.Add("jfif", "image/jpeg");
            _mimeTypes.Add("jfif-tbnl", "image/jpeg");
            _mimeTypes.Add("jpe", "image/jpeg");
            _mimeTypes.Add("jpeg", "image/jpeg");
            _mimeTypes.Add("jpg", "image/jpeg");
            _mimeTypes.Add("jps", "image/x-jps");
            _mimeTypes.Add("js", "application/x-javascript");
            _mimeTypes.Add("jut", "image/jutvision");
            _mimeTypes.Add("kar", "audio/midi");
            _mimeTypes.Add("ksh", "application/x-ksh");
            _mimeTypes.Add("la", "audio/nspaudio");
            _mimeTypes.Add("lam", "audio/x-liveaudio");
            _mimeTypes.Add("latex", "application/x-latex");
            _mimeTypes.Add("lha", "application/lha");
            _mimeTypes.Add("lhx", "application/octet-stream");
            _mimeTypes.Add("list", "text/plain");
            _mimeTypes.Add("lma", "audio/nspaudio");
            _mimeTypes.Add("log", "text/plain");
            _mimeTypes.Add("lsp", "application/x-lisp");
            _mimeTypes.Add("lst", "text/plain");
            _mimeTypes.Add("lsx", "text/x-la-asf");
            _mimeTypes.Add("ltx", "application/x-latex");
            _mimeTypes.Add("lzh", "application/octet-stream");
            _mimeTypes.Add("lzx", "application/lzx");
            _mimeTypes.Add("m", "text/plain");
            _mimeTypes.Add("m1v", "video/mpeg");
            _mimeTypes.Add("m2a", "audio/mpeg");
            _mimeTypes.Add("m2v", "video/mpeg");
            _mimeTypes.Add("m3u", "audio/x-mpequrl");
            _mimeTypes.Add("man", "application/x-troff-man");
            _mimeTypes.Add("map", "application/x-navimap");
            _mimeTypes.Add("mar", "text/plain");
            _mimeTypes.Add("mbd", "application/mbedlet");
            _mimeTypes.Add("mc$", "application/x-magic-cap-package-1.0");
            _mimeTypes.Add("mcd", "application/mcad");
            _mimeTypes.Add("mcf", "image/vasa");
            _mimeTypes.Add("mcp", "application/netmc");
            _mimeTypes.Add("me", "application/x-troff-me");
            _mimeTypes.Add("mht", "message/rfc822");
            _mimeTypes.Add("mhtml", "message/rfc822");
            _mimeTypes.Add("mid", "audio/midi");
            _mimeTypes.Add("midi", "audio/midi");
            _mimeTypes.Add("mif", "application/x-frame");
            _mimeTypes.Add("mime", "message/rfc822");
            _mimeTypes.Add("mjf", "audio/x-vnd.audioexplosion.mjuicemediafile");
            _mimeTypes.Add("mjpg", "video/x-motion-jpeg");
            _mimeTypes.Add("mm", "application/base64");
            _mimeTypes.Add("mme", "application/base64");
            _mimeTypes.Add("mod", "audio/mod");
            _mimeTypes.Add("moov", "video/quicktime");
            _mimeTypes.Add("mov", "video/quicktime");
            _mimeTypes.Add("movie", "video/x-sgi-movie");
            _mimeTypes.Add("mp2", "audio/mpeg");
            _mimeTypes.Add("mp3", "audio/mpeg3");
            _mimeTypes.Add("mpa", "audio/mpeg");
            _mimeTypes.Add("mpc", "application/x-project");
            _mimeTypes.Add("mpe", "video/mpeg");
            _mimeTypes.Add("mpeg", "video/mpeg");
            _mimeTypes.Add("mpg", "video/mpeg");
            _mimeTypes.Add("mpga", "audio/mpeg");
            _mimeTypes.Add("mpp", "application/vnd.ms-project");
            _mimeTypes.Add("mpt", "application/x-project");
            _mimeTypes.Add("mpv", "application/x-project");
            _mimeTypes.Add("mpx", "application/x-project");
            _mimeTypes.Add("mrc", "application/marc");
            _mimeTypes.Add("ms", "application/x-troff-ms");
            _mimeTypes.Add("mv", "video/x-sgi-movie");
            _mimeTypes.Add("my", "audio/make");
            _mimeTypes.Add("mzz", "application/x-vnd.audioexplosion.mzz");
            _mimeTypes.Add("nap", "image/naplps");
            _mimeTypes.Add("naplps", "image/naplps");
            _mimeTypes.Add("nc", "application/x-netcdf");
            _mimeTypes.Add("ncm", "application/vnd.nokia.configuration-message");
            _mimeTypes.Add("nif", "image/x-niff");
            _mimeTypes.Add("niff", "image/x-niff");
            _mimeTypes.Add("nix", "application/x-mix-transfer");
            _mimeTypes.Add("nsc", "application/x-conference");
            _mimeTypes.Add("nvd", "application/x-navidoc");
            _mimeTypes.Add("o", "application/octet-stream");
            _mimeTypes.Add("oda", "application/oda");
            _mimeTypes.Add("omc", "application/x-omc");
            _mimeTypes.Add("omcd", "application/x-omcdatamaker");
            _mimeTypes.Add("omcr", "application/x-omcregerator");
            _mimeTypes.Add("p", "text/x-pascal");
            _mimeTypes.Add("p10", "application/pkcs10");
            _mimeTypes.Add("p12", "application/pkcs-12");
            _mimeTypes.Add("p7a", "application/x-pkcs7-signature");
            _mimeTypes.Add("p7c", "application/pkcs7-mime");
            _mimeTypes.Add("pas", "text/pascal");
            _mimeTypes.Add("pbm", "image/x-portable-bitmap");
            _mimeTypes.Add("pcl", "application/vnd.hp-pcl");
            _mimeTypes.Add("pct", "image/x-pict");
            _mimeTypes.Add("pcx", "image/x-pcx");
            _mimeTypes.Add("pdf", "application/pdf");
            _mimeTypes.Add("pfunk", "audio/make");
            _mimeTypes.Add("pgm", "image/x-portable-graymap");
            _mimeTypes.Add("pic", "image/pict");
            _mimeTypes.Add("pict", "image/pict");
            _mimeTypes.Add("pkg", "application/x-newton-compatible-pkg");
            _mimeTypes.Add("pko", "application/vnd.ms-pki.pko");
            _mimeTypes.Add("pl", "text/plain");
            _mimeTypes.Add("plx", "application/x-pixclscript");
            _mimeTypes.Add("pm", "image/x-xpixmap");
            _mimeTypes.Add("png", "image/png");
            _mimeTypes.Add("pnm", "application/x-portable-anymap");
            _mimeTypes.Add("pot", "application/mspowerpoint");
            _mimeTypes.Add("pov", "model/x-pov");
            _mimeTypes.Add("ppa", "application/vnd.ms-powerpoint");
            _mimeTypes.Add("ppm", "image/x-portable-pixmap");
            _mimeTypes.Add("pps", "application/mspowerpoint");
            _mimeTypes.Add("ppt", "application/mspowerpoint");
            _mimeTypes.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            _mimeTypes.Add("ppz", "application/mspowerpoint");
            _mimeTypes.Add("pre", "application/x-freelance");
            _mimeTypes.Add("prt", "application/pro_eng");
            _mimeTypes.Add("ps", "application/postscript");
            _mimeTypes.Add("psd", "application/octet-stream");
            _mimeTypes.Add("pvu", "paleovu/x-pv");
            _mimeTypes.Add("pwz", "application/vnd.ms-powerpoint");
            _mimeTypes.Add("py", "text/x-script.phyton");
            _mimeTypes.Add("pyc", "applicaiton/x-bytecode.python");
            _mimeTypes.Add("qcp", "audio/vnd.qcelp");
            _mimeTypes.Add("qd3", "x-world/x-3dmf");
            _mimeTypes.Add("qd3d", "x-world/x-3dmf");
            _mimeTypes.Add("qif", "image/x-quicktime");
            _mimeTypes.Add("qt", "video/quicktime");
            _mimeTypes.Add("qtc", "video/x-qtc");
            _mimeTypes.Add("qti", "image/x-quicktime");
            _mimeTypes.Add("qtif", "image/x-quicktime");
            _mimeTypes.Add("ra", "audio/x-pn-realaudio");
            _mimeTypes.Add("ram", "audio/x-pn-realaudio");
            _mimeTypes.Add("ras", "application/x-cmu-raster");
            _mimeTypes.Add("rast", "image/cmu-raster");
            _mimeTypes.Add("rexx", "text/x-script.rexx");
            _mimeTypes.Add("rf", "image/vnd.rn-realflash");
            _mimeTypes.Add("rgb", "image/x-rgb");
            _mimeTypes.Add("rm", "application/vnd.rn-realmedia");
            _mimeTypes.Add("rmi", "audio/mid");
            _mimeTypes.Add("rmm", "audio/x-pn-realaudio");
            _mimeTypes.Add("rmp", "audio/x-pn-realaudio");
            _mimeTypes.Add("rng", "application/ringing-tones");
            _mimeTypes.Add("rnx", "application/vnd.rn-realplayer");
            _mimeTypes.Add("roff", "application/x-troff");
            _mimeTypes.Add("rp", "image/vnd.rn-realpix");
            _mimeTypes.Add("rpm", "audio/x-pn-realaudio-plugin");
            _mimeTypes.Add("rt", "text/richtext");
            _mimeTypes.Add("rtf", "text/richtext");
            _mimeTypes.Add("rtx", "application/rtf");
            _mimeTypes.Add("rv", "video/vnd.rn-realvideo");
            _mimeTypes.Add("s", "text/x-asm");
            _mimeTypes.Add("s3m", "audio/s3m");
            _mimeTypes.Add("saveme", "application/octet-stream");
            _mimeTypes.Add("sbk", "application/x-tbook");
            _mimeTypes.Add("scm", "application/x-lotusscreencam");
            _mimeTypes.Add("sdml", "text/plain");
            _mimeTypes.Add("sdp", "application/sdp");
            _mimeTypes.Add("sdr", "application/sounder");
            _mimeTypes.Add("sea", "application/sea");
            _mimeTypes.Add("set", "application/set");
            _mimeTypes.Add("sgm", "text/sgml");
            _mimeTypes.Add("sgml", "text/sgml");
            _mimeTypes.Add("sh", "application/x-bsh");
            _mimeTypes.Add("shtml", "text/html");
            _mimeTypes.Add("sid", "audio/x-psid");
            _mimeTypes.Add("sit", "application/x-sit");
            _mimeTypes.Add("skd", "application/x-koan");
            _mimeTypes.Add("skm", "application/x-koan");
            _mimeTypes.Add("skp", "application/x-koan");
            _mimeTypes.Add("skt", "application/x-koan");
            _mimeTypes.Add("sl", "application/x-seelogo");
            _mimeTypes.Add("smi", "application/smil");
            _mimeTypes.Add("smil", "application/smil");
            _mimeTypes.Add("snd", "audio/basic");
            _mimeTypes.Add("sol", "application/solids");
            _mimeTypes.Add("spc", "application/x-pkcs7-certificates");
            _mimeTypes.Add("spl", "application/futuresplash");
            _mimeTypes.Add("spr", "application/x-sprite");
            _mimeTypes.Add("sprite", "application/x-sprite");
            _mimeTypes.Add("src", "application/x-wais-source");
            _mimeTypes.Add("ssi", "text/x-server-parsed-html");
            _mimeTypes.Add("ssm", "application/streamingmedia");
            _mimeTypes.Add("sst", "application/vnd.ms-pki.certstore");
            _mimeTypes.Add("step", "application/step");
            _mimeTypes.Add("stl", "application/sla");
            _mimeTypes.Add("stp", "application/step");
            _mimeTypes.Add("sv4cpio", "application/x-sv4cpio");
            _mimeTypes.Add("sv4crc", "application/x-sv4crc");
            _mimeTypes.Add("svf", "image/vnd.dwg");
            _mimeTypes.Add("svr", "application/x-world");
            _mimeTypes.Add("swf", "application/x-shockwave-flash");
            _mimeTypes.Add("t", "application/x-troff");
            _mimeTypes.Add("talk", "text/x-speech");
            _mimeTypes.Add("tar", "application/x-tar");
            _mimeTypes.Add("tbk", "application/toolbook");
            _mimeTypes.Add("tcl", "application/x-tcl");
            _mimeTypes.Add("tcsh", "text/x-script.tcsh");
            _mimeTypes.Add("tex", "application/x-tex");
            _mimeTypes.Add("texi", "application/x-texinfo");
            _mimeTypes.Add("texinfo", "application/x-texinfo");
            _mimeTypes.Add("text", "text/plain");
            _mimeTypes.Add("tgz", "application/x-compressed");
            _mimeTypes.Add("tif", "image/tiff");
            _mimeTypes.Add("tr", "application/x-troff");
            _mimeTypes.Add("tsi", "audio/tsp-audio");
            _mimeTypes.Add("tsp", "audio/tsplayer");
            _mimeTypes.Add("tsv", "text/tab-separated-values");
            _mimeTypes.Add("turbot", "image/florian");
            _mimeTypes.Add("txt", "text/plain");
            _mimeTypes.Add("uil", "text/x-uil");
            _mimeTypes.Add("uni", "text/uri-list");
            _mimeTypes.Add("unis", "text/uri-list");
            _mimeTypes.Add("unv", "application/i-deas");
            _mimeTypes.Add("uri", "text/uri-list");
            _mimeTypes.Add("uris", "text/uri-list");
            _mimeTypes.Add("ustar", "application/x-ustar");
            _mimeTypes.Add("uu", "application/octet-stream");
            _mimeTypes.Add("vcd", "application/x-cdlink");
            _mimeTypes.Add("vcs", "text/x-vcalendar");
            _mimeTypes.Add("vda", "application/vda");
            _mimeTypes.Add("vdo", "video/vdo");
            _mimeTypes.Add("vew", "application/groupwise");
            _mimeTypes.Add("viv", "video/vivo");
            _mimeTypes.Add("vivo", "video/vivo");
            _mimeTypes.Add("vmd", "application/vocaltec-media-desc");
            _mimeTypes.Add("vmf", "application/vocaltec-media-file");
            _mimeTypes.Add("voc", "audio/voc");
            _mimeTypes.Add("vos", "video/vosaic");
            _mimeTypes.Add("vox", "audio/voxware");
            _mimeTypes.Add("vqe", "audio/x-twinvq-plugin");
            _mimeTypes.Add("vqf", "audio/x-twinvq");
            _mimeTypes.Add("vql", "audio/x-twinvq-plugin");
            _mimeTypes.Add("vrml", "application/x-vrml");
            _mimeTypes.Add("vrt", "x-world/x-vrt");
            _mimeTypes.Add("vsd", "application/x-visio");
            _mimeTypes.Add("vst", "application/x-visio");
            _mimeTypes.Add("vsw", "application/x-visio");
            _mimeTypes.Add("w60", "application/wordperfect6.0");
            _mimeTypes.Add("w61", "application/wordperfect6.1");
            _mimeTypes.Add("w6w", "application/msword");
            _mimeTypes.Add("wav", "audio/wav");
            _mimeTypes.Add("wb1", "application/x-qpro");
            _mimeTypes.Add("wbmp", "image/vnd.wap.wbmp");
            _mimeTypes.Add("web", "application/vnd.xara");
            _mimeTypes.Add("wiz", "application/msword");
            _mimeTypes.Add("wk1", "application/x-123");
            _mimeTypes.Add("wmf", "windows/metafile");
            _mimeTypes.Add("wml", "text/vnd.wap.wml");
            _mimeTypes.Add("wmlc", "application/vnd.wap.wmlc");
            _mimeTypes.Add("wmls", "text/vnd.wap.wmlscript");
            _mimeTypes.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            _mimeTypes.Add("word", "application/msword");
            _mimeTypes.Add("wp", "application/wordperfect");
            _mimeTypes.Add("wp5", "application/wordperfect");
            _mimeTypes.Add("wp6", "application/wordperfect");
            _mimeTypes.Add("wpd", "application/wordperfect");
            _mimeTypes.Add("wq1", "application/x-lotus");
            _mimeTypes.Add("wri", "application/mswrite");
            _mimeTypes.Add("wrl", "application/x-world");
            _mimeTypes.Add("wrz", "model/vrml");
            _mimeTypes.Add("wsc", "text/scriplet");
            _mimeTypes.Add("wsrc", "application/x-wais-source");
            _mimeTypes.Add("wtk", "application/x-wintalk");
            _mimeTypes.Add("xbm", "image/x-xbitmap");
            _mimeTypes.Add("xdr", "video/x-amt-demorun");
            _mimeTypes.Add("xgz", "xgl/drawing");
            _mimeTypes.Add("xif", "image/vnd.xiff");
            _mimeTypes.Add("xl", "application/excel");
            _mimeTypes.Add("xla", "application/excel");
            _mimeTypes.Add("xlb", "application/excel");
            _mimeTypes.Add("xlc", "application/excel");
            _mimeTypes.Add("xld", "application/excel");
            _mimeTypes.Add("xlk", "application/excel");
            _mimeTypes.Add("xll", "application/excel");
            _mimeTypes.Add("xlm", "application/excel");
            _mimeTypes.Add("xls", "application/excel");
            _mimeTypes.Add("xlsx", "application/excel");
            _mimeTypes.Add("xlt", "application/excel");
            _mimeTypes.Add("xlv", "application/excel");
            _mimeTypes.Add("xlw", "application/excel");
            _mimeTypes.Add("xm", "audio/xm");
            _mimeTypes.Add("xml", "text/xml");
            _mimeTypes.Add("xmz", "xgl/movie");
            _mimeTypes.Add("xpix", "application/x-vnd.ls-xpix");
            _mimeTypes.Add("xpm", "image/x-xpixmap");
            _mimeTypes.Add("x-png", "image/png");
            _mimeTypes.Add("xsr", "video/x-amt-showrun");
            _mimeTypes.Add("xwd", "image/x-xwd");
            _mimeTypes.Add("xyz", "chemical/x-pdb");
            _mimeTypes.Add("z", "application/x-compress");
            _mimeTypes.Add("zip", "application/x-compressed");
            _mimeTypes.Add("zoo", "application/octet-stream");
            _mimeTypes.Add("zsh", "text/x-script.zsh");
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
            var res = new List<SelectListItem>()
            {
                new(nullContent, "")
            };
            
            if (lstSource != null && lstSource.Any())
                res.AddRange(lstSource);
            if (current.HasValue)
            {
                foreach (var itm in lstSource)
                {
                    if (!string.IsNullOrEmpty(itm.Value))
                        itm.Selected = Convert.ToInt64(itm.Value) == current.Value;
                }
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
        public static void BuildRecursiveItem(ref List<NestedItem> lstSource)
        {
            // List parent
            var lstParent = lstSource.Where(o => o.ParentId == null).ToList();

            foreach (var item in lstParent)
            {
                AddChildItem(lstSource, item);
            }

            lstSource = lstParent;
        }
        
        private static void AddChildItem(IEnumerable<NestedItem> lstSource, NestedItem item)
        {
            var enumerable = lstSource.ToList();
            var childItems = enumerable.Where(o => o.ParentId == item.Id && o.Id > 0).ToList();
            if (childItems.Count <= 0) return;
            foreach (var cItem in childItems)
            {
                item.Children ??= new List<NestedItem>();
                AddChildItem(enumerable, cItem);
                item.Children.Add(cItem);
            }
        }
    }
}