using System.Collections.Generic;
using Abp.AutoMapper;
using Zero.MultiTenancy.Dto;
using Zero.Sessions.Dto;

namespace Zero.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
        
        public List<TenantListDto> AvailableTenants { get; set; }
    }
}