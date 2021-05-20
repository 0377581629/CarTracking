using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Zero.Authorization.Permissions.Dto;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}