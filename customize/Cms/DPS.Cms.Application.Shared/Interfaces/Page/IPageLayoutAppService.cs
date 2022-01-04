using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.PageLayout;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IPageLayoutAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPageLayoutForViewDto>> GetAll(GetAllPageLayoutInput input);
        
        Task<GetPageLayoutForEditOutput> GetPageLayoutForEdit(EntityDto input);
        
		Task CreateOrEdit(CreateOrEditPageLayoutDto input);
        
		Task Delete(EntityDto input);
        
        Task<PageLayoutConfigDto> GetPageLayoutForConfig(EntityDto input);
        
        Task UpdateConfig(PageLayoutConfigDto input);
    }
}