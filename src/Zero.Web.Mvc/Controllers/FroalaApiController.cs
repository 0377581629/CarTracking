using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.Threading;
using Abp.Web.Models;
using FroalaEditor;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.Exceptions;
using Zero;
using Zero.Debugging;
using Zero.Web.Controllers;
using FileOptions = FroalaEditor.FileOptions;

namespace ZERO.Web.Controllers
{
    [AbpMvcAuthorize]
    [DontWrapResult]
    public class FroalaApiController : ZeroControllerBase
    {
        private readonly IFileAppService _fileService;

        public FroalaApiController(IFileAppService fileService)
        {
            _fileService = fileService;
        }

        public IActionResult UploadImage()
        {
            try
            {
                return Json(Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image)));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadVideo()
        {
            try
            {
                return Json(Video.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Video)));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadFile()
        {
            object response;
            try
            {
                response = FroalaEditor.File.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Office));
                return Json(response);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult LoadImages()
        {
            try
            {
                return Json(Image.List(FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image)));
            }
            catch (MinioException e)
            {
                Console.WriteLine("Error occurred: " + e);
                return Json(e);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadImageResize()
        {
            var resizeGeometry = new MagickGeometry(300, 300) { IgnoreAspectRatio = true };

            var options = new ImageOptions
            {
                ResizeGeometry = resizeGeometry
            };

            try
            {
                return Json(Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image), options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadImageValidation()
        {
            bool ValidationFunction(string filePath, string mimeType)
            {
                var info = new MagickImageInfo(filePath);

                return info.Width == info.Height;
            }

            var options = new ImageOptions
            {
                Fieldname = "myImage",
                Validation = new ImageValidation(ValidationFunction)
            };

            try
            {
                return Json(Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image), options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadFileValidation()
        {
            bool ValidationFunction(string filePath, string mimeType)
            {
                var size = new FileInfo(filePath).Length;
                if (size > 10 * 1024 * 1024)
                {
                    return false;
                }

                return true;
            }

            var options = new FileOptions
            {
                Fieldname = "myFile",
                Validation = new FileValidation(ValidationFunction)
            };

            try
            {
                return Json(Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Office), options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult DeleteFile()
        {
            try
            {
                FroalaEditor.File.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult DeleteVideo()
        {
            try
            {
                Video.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult DeleteImage()
        {
            try
            {
                Image.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult S3Signature()
        {
            var config = new S3Config
            {
                Bucket = Environment.GetEnvironmentVariable("AWS_BUCKET"),
                Region = Environment.GetEnvironmentVariable("AWS_REGION"),
                KeyStart = Environment.GetEnvironmentVariable("AWS_KEY_START"),
                Acl = Environment.GetEnvironmentVariable("AWS_ACL"),
                AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY"),
                Expiration = Environment.GetEnvironmentVariable("AWS_EXPIRATION") // Expiration s3 image signature #11
            };

            return Json(S3.GetHash(config));
        }

        public IActionResult Error()
        {
            return View("/Views/Error/Error.cshtml");
        }
    }
}