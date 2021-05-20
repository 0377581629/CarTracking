using System.Collections.Generic;
using MvvmHelpers;
using Zero.Models.NavigationMenu;

namespace Zero.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}