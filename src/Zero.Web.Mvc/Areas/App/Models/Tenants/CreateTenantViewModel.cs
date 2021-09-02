using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Editions.Dto;
using Zero.Security;

namespace Zero.Web.Areas.App.Models.Tenants
{
    public class CreateTenantViewModel
    {
        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateTenantViewModel(IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            EditionItems = editionItems;
        }
        
        public List<SelectListItem> ListTenant { get; set; }
    }
}