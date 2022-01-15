using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.FileManager.Model;

namespace Zero.Web.FileManager.Interfaces
{
    public interface IFileManagerController
    {
        Task<JsonResult> Read(string target, string filter);

        ActionResult Destroy(FileManagerViewModel viewModel);

        ActionResult Create(string target, FileManagerViewModel viewModel);

        ActionResult Update(string target, FileManagerViewModel viewModel);

        ActionResult Upload(string path, IFormFile file);
    }
}