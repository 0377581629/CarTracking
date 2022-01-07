using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.Json;
using Abp.Threading;
using Abp.Web.Models;
using FroalaEditor;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using Zero;
using Zero.Debugging;
using Zero.Web.Controllers;

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
                return Json(FroalaEditor.Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image)));
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
                return Json(FroalaEditor.Video.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Video)));
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
                if (!SystemConfig.UseFileServer) 
                    return Json(FroalaEditor.Image.List(FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image)));

                var minioClient = new MinioClient(SystemConfig.MinioEndPoint,
                    SystemConfig.MinioAccessKey,
                    SystemConfig.MinioSecretKey);
                if (!DebugHelper.IsDebug)
                    minioClient = minioClient.WithSSL();

                var rootBucket = AsyncHelper.RunSync(() => _fileService.RootFileServerBucketName());
                
                if (!AsyncHelper.RunSync(() => minioClient.BucketExistsAsync(rootBucket)))
                {
                    AsyncHelper.RunSync(() => minioClient.MakeBucketAsync(rootBucket));
                }

                var lstImages = new List<ImageModel>();
                var observable = minioClient.ListObjectsAsync(rootBucket, recursive: true);
                observable.Subscribe(
                    item =>
                    {
                        if (!item.IsDir && ImageValidation.AllowedImageExtsDefault.Contains(Path.GetExtension(item.Key).RemovePreFix(".")))
                        {
                            lstImages.Add(new ImageModel
                            {
                                Url = $"http://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{item.Key}",
                                Thumb = $"http://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{item.Key}"
                            });
                        }        
                    },
                    ex => Console.WriteLine("Minio Error: {0}", ex.Message));
                observable.Wait();
                return Json(lstImages);
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

            var options = new FroalaEditor.ImageOptions
            {
                ResizeGeometry = resizeGeometry
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image), options));
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

            var options = new FroalaEditor.ImageOptions
            {
                Fieldname = "myImage",
                Validation = new FroalaEditor.ImageValidation(ValidationFunction)
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Image), options));
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
                var size = new System.IO.FileInfo(filePath).Length;
                if (size > 10 * 1024 * 1024)
                {
                    return false;
                }

                return true;
            }

            var options = new FroalaEditor.FileOptions
            {
                Fieldname = "myFile",
                Validation = new FroalaEditor.FileValidation(ValidationFunction)
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(HttpContext, FileHelper.UploadPath(AbpSession, ZeroEnums.FileType.Office), options));
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
                FroalaEditor.Video.Delete(HttpContext.Request.Form["src"]);
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
                FroalaEditor.Image.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult S3Signature()
        {
            var config = new FroalaEditor.S3Config
            {
                Bucket = Environment.GetEnvironmentVariable("AWS_BUCKET"),
                Region = Environment.GetEnvironmentVariable("AWS_REGION"),
                KeyStart = Environment.GetEnvironmentVariable("AWS_KEY_START"),
                Acl = Environment.GetEnvironmentVariable("AWS_ACL"),
                AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY"),
                Expiration = Environment.GetEnvironmentVariable("AWS_EXPIRATION") // Expiration s3 image signature #11
            };

            return Json(FroalaEditor.S3.GetHash(config));
        }

        public IActionResult Error()
        {
            return View("/Views/Error/Error.cshtml");
        }
    }
}