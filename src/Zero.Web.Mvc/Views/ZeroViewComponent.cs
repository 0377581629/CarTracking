using Abp.AspNetCore.Mvc.ViewComponents;

namespace Zero.Web.Views
{
    public abstract class ZeroViewComponent : AbpViewComponent
    {
        protected ZeroViewComponent()
        {
            LocalizationSourceName = ZeroConst.LocalizationSourceName;
        }
    }
}