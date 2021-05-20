using System.Collections.Generic;
using Zero.Authorization.Delegation;
using Zero.Authorization.Users.Delegation.Dto;

namespace Zero.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
