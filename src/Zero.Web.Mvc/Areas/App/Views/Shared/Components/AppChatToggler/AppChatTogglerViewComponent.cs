using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Areas.App.Models.Layout;
using Zero.Web.Views;

namespace Zero.Web.Areas.App.Views.Shared.Components.AppChatToggler
{
    public class AppChatTogglerViewComponent : ZeroViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            return Task.FromResult<IViewComponentResult>(View(new ChatTogglerViewModel
            {
                CssClass = cssClass
            }));
        }
    }
}
