using Abp.AutoMapper;
using Abp.Domain.Entities;
using DPS.Cms.Application.Shared.Dto.Menu;

namespace Zero.Web.Areas.Cms.Models.Menu
{
    [AutoMapFrom(typeof(DPS.Cms.Core.Menu.Menu))]
    public class CreateOrEditMenuViewModel: Entity<int?>
    {
        public int? ParentId { get; set; }

        public CreateOrEditMenuViewModel(int? parentId)
        {
            ParentId = parentId;
        }
        
        public string Code { get; set; }

        public string Name { get; set; }
        
        public string Url { get; set; }
        
        public int MenuGroupId { get; set; }
        
        public int MenuGroupCode { get; set; }
        
        public int MenuGroupName { get; set; }
        
        public bool IsEditMode => Id.HasValue;
    }
}