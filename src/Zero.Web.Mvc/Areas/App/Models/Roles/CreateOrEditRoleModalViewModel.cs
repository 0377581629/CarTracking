using System.Collections.Generic;
using Abp.AutoMapper;
using Zero.Authorization.Roles.Dto;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
        
        public List<DashboardWidgetDto> DashboardWidgets { get; set; }
        
        public List<DashboardWidgetDto> GrantedDashboardWidgets { get; set; }
    }
}