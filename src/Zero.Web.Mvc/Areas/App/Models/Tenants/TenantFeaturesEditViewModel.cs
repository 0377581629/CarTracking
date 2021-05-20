using Abp.AutoMapper;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}