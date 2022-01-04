using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Page;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IPageAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPageForViewDto>> GetAll(GetAllPageInput input);
        
        Task<GetPageForEditOutput> GetPageForEdit(EntityDto input);
        
		Task CreateOrEdit(CreateOrEditPageDto input);
        
        Task Delete(EntityDto input);
        
        Task<PageConfigDto> GetPageConfig(EntityDto input);
        
        Task<PageWidgetDto> GetPageWidgetForEdit(EntityDto input);
        
        Task UpdatePageDetails(PageConfigDto input);
    }
}