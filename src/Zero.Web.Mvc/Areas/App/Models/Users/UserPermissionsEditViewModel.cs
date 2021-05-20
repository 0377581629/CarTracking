using Abp.AutoMapper;
using Zero.Authorization.Users;
using Zero.Authorization.Users.Dto;
using Zero.Web.Areas.App.Models.Common;

namespace Zero.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}