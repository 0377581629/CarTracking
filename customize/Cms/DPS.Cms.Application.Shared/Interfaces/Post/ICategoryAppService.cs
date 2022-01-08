using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Category;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface ICategoryAppService : IApplicationService 
    {
	    Task<List<NestedItem>> GetAllNested();
	    
	    Task<ListResultDto<CategoryDto>> GetCategories();
        
	    Task<NestedItem> CreateOrEditCategory(CreateOrEditCategoryDto input);

	    Task UpdateStructure(UpdateCategoryStructureInput input);
	    
	    Task Delete(EntityDto input);
    }
}