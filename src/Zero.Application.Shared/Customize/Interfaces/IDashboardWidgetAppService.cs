using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Customize.Dto.Dashboard.DashboardWidget;

namespace Zero.Customize.Interfaces
{
    public interface IDashboardWidgetAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDashboardWidgetForViewDto>> GetAll(GetAllDashboardWidgetInput input);
        
        Task<GetDashboardWidgetForEditOutput> GetDashboardWidgetForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDashboardWidgetDto input);

		Task Delete(EntityDto input);

        Task<List<DashboardWidgetDto>> GetApp();
    }
}