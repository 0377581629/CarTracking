using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Menu;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Shared.Interfaces.Menu
{
    public interface IMenuAppService: IApplicationService 
    {
        Task<List<NestedItem>> GetAllNested(GetAllMenuInput input);
	    
        Task<ListResultDto<MenuDto>> GetMenus();
        
        Task<NestedItem> CreateOrEditMenu(CreateOrEditMenuDto input);

        Task UpdateStructure(UpdateMenuStructureInput input);
	    
        Task Delete(EntityDto<int> input);
    }
}