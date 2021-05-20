using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Areas.App.Models.Layout;
using Zero.Web.Views;

namespace Zero.Web.Areas.App.Views.Shared.Components.
    AppQuickThemeSelect
{
    public class AppQuickThemeSelectViewComponent : ZeroViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            return Task.FromResult<IViewComponentResult>(View(new QuickThemeSelectionViewModel
            {
                CssClass = cssClass
            }));
        }
    }
}
