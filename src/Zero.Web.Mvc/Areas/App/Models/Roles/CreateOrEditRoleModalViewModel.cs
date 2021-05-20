using Abp.AutoMapper;
using Zero.Authorization.Roles.Dto;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}