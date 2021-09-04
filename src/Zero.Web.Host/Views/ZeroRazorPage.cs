using Abp.AspNetCore.Mvc.Views;

namespace Zero.Web.Views
{
    public abstract class ZeroRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ZeroRazorPage()
        {
            LocalizationSourceName = ZeroConsts.LocalizationSourceName;
        }
    }
}
