using Abp.AspNetCore.Mvc.ViewComponents;

namespace Zero.Web.Public.Views
{
    public abstract class ZeroViewComponent : AbpViewComponent
    {
        protected ZeroViewComponent()
        {
            LocalizationSourceName = ZeroConst.LocalizationSourceName;
        }
    }
}