using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Zero.Web.Public.Views
{
    public abstract class ZeroRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ZeroRazorPage()
        {
            LocalizationSourceName = ZeroConsts.LocalizationSourceName;
        }
    }
}
