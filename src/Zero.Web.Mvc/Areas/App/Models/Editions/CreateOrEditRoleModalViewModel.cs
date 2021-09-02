using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Zero.Authorization.Permissions.Dto;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Editions.Dto;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class CreateEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel, IPermissionsEditViewModel
    {
        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
        
        public List<FlatPermissionDto> Permissions { get; set; }
        
        public List<string> GrantedPermissionNames { get; set; }
        
        public List<DashboardWidgetDto> DashboardWidgets { get; set; }
        
        public List<DashboardWidgetDto> GrantedDashboardWidgets { get; set; }
    }
}