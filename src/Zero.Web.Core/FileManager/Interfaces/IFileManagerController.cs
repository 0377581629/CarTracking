using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.FileManager.Model;

namespace Zero.Web.FileManager.Interfaces
{
    public interface IFileManagerController
    {
        Task<JsonResult> Read(string target, string filter);

        Task<ActionResult> Destroy(FileManagerViewModel viewModel);

        Task<ActionResult> Create(string target, FileManagerViewModel viewModel);

        Task<ActionResult> Update(string target, FileManagerViewModel viewModel);

        Task<ActionResult> Upload(string path, IFormFile file);
    }
}