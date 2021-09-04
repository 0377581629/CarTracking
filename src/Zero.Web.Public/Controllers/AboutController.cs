using Microsoft.AspNetCore.Mvc;
using Zero.Web.Controllers;

namespace Zero.Web.Public.Controllers
{
    public class AboutController : ZeroControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}