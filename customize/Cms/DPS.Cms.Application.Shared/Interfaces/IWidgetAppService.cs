using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Cms.Application.Shared.Dto.Widget;

namespace DPS.Cms.Application.Shared.Interfaces
{
    public interface IWidgetAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWidgetForViewDto>> GetAll(GetAllWidgetInput input);
        
        Task<GetWidgetForEditOutput> GetWidgetForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditWidgetDto input);

		Task Delete(EntityDto input);
    }
}