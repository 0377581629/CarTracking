using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Dto;

namespace Zero.Web.Areas.App.Models.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
        }
        
        public List<SelectListItem> ListTenant { get; set; }
    }
}