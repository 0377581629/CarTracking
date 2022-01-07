using ImageMagick;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace FroalaEditor
{
    /// <summary>
    /// File functionality.
    /// </summary>
    public static class File
    {
        /// <summary>
        /// Content type string used in http multipart.
        /// </summary>
        public static string MultipartContentType = "multipart/form-data";

        /// <summary>
        /// File default options.
        /// </summary>
        public static FileOptions defaultOptions = new FileOptions();

        /// <summary>
        /// Check http request content type.
        /// </summary>
        /// <returns>true if content type is multipart.</returns>
        public static bool CheckContentType(HttpContext httpContext)
        {       
            var isMultipart = httpContext.Request.ContentType.StartsWith(MultipartContentType);

            return isMultipart;
        }

        /// <summary>
        /// Uploads a file to disk.
        /// </summary>
        /// <param name="httpContext">The HttpContext object containing information about the request.</param>
        /// <param name="fileRoute">Server route where the file will be uploaded. This route must be public to be accesed by the editor.</param>
        /// <param name="options">File options.</param>
        /// <returns>Object with link.</returns>
        public static object Upload(HttpContext httpContext, string fileRoute, FileOptions options = null)
        {
            // Use default file options.
            if (options == null)
            {
                options = defaultOptions;
            }

            if (!CheckContentType(httpContext))
            {
                throw new Exception("Invalid contentType. It must be " + MultipartContentType);
            }

            var httpRequest = httpContext.Request;

            var filesCount = 0;
            filesCount = httpRequest.Form.Files.Count;


            if (filesCount == 0)
            {
                throw new Exception("No file found");
            }

            // Get HTTP posted file based on the fieldname. 
            var file = httpRequest.Form.Files.GetFile(options.Fieldname);
            
            if (file == null)
            {
                throw new Exception("Fieldname is not correct. It must be: " + options.Fieldname);
            }

            // Generate Random name.
            var extension = Utils.GetFileExtension(file.FileName);

            var name = $"{Utils.GenerateUniqueString()}.{extension}";

            var link = fileRoute + name; 

            // Bug Fixes in File.cs #2
            // https://github.com/froala/wysiwyg-editor-dotnet-sdk/issues/2
            // Create directory if it doesn't exist.
            var fileRoutePath = new FileInfo(GetAbsoluteServerPath(fileRoute));
            if (fileRoutePath.Directory != null && !fileRoutePath.Directory.Exists)
            {
                fileRoutePath.Directory.Create();
            }

            // Copy contents to memory stream.
            Stream stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            var serverPath = GetAbsoluteServerPath(link);

            // Save file to disk.
            Save(stream, serverPath, options);

            // Check if the file is valid.
            if (options.Validation != null && !options.Validation.Check(serverPath, file.ContentType))
            {
                // Bug Fixes in File.cs #2
                // https://github.com/froala/wysiwyg-editor-dotnet-sdk/issues/2
                // Delete "link"
                Delete(link);
                throw new Exception("File does not meet the validation.");
            }

            // Make sure it is compatible with ASP.NET Core.
            return new { link = link.Replace("wwwroot/", "/") };
        }

        /// <summary>
        /// Get absolute server path.
        /// </summary>
        /// <param name="path">Relative path.</param>
        /// <returns>Absolute path.</returns>
        public static string GetAbsoluteServerPath(string path) 
        {
            return path;
        }

        /// <summary>
        /// Save an input file stream to disk.
        /// </summary>
        /// <param name="fileStream">Input file stream</param>
        /// <param name="link">Server file path.</param>
        /// <param name="options">File options.</param>
        public static void Save(Stream fileStream, string filePath, FileOptions options)
        {
            if (options is ImageOptions && ((ImageOptions)options).ResizeGeometry != null)
            {
                // Resize file and save it..
                var image = new MagickImage(fileStream);
                image.Resize(((ImageOptions)options).ResizeGeometry);
                image.Write(filePath);
            }
            else
            {
                // Save file to disk.
                Console.WriteLine(filePath);
                var writerfileStream = System.IO.File.Create(filePath);
                fileStream.CopyTo(writerfileStream);
                writerfileStream.Dispose();
            }
        }

        /// <summary>
        /// Delete a file from disk.
        /// </summary>
        /// <param name="filePath">Server file path.</param>
        public static void Delete(string filePath)
        {
            // Will throw an exception if an error occurs.
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
