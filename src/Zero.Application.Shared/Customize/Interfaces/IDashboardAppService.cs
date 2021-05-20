using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Customize.Dto.Dashboard.Config;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Dto.Dashboard.Function;

namespace Zero.Customize.Interfaces
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<List<DashboardWidgetDto>> GetAllDashboardWidget();

        Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByEdition(int editionId);
        
        Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByRole(int roleId);
        
        Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByRoles(List<int> roleIds);
        
        Task<Dashboard> GetUserDashboard();
        
        Task SavePage(SavePageInput input);

        Task RenamePage(RenamePageInput input);

        Task<AddNewPageOutput> AddNewPage(AddNewPageInput input);

        Task<DashboardWidgetDto> AddWidget(AddWidgetInput input);

        Task DeletePage(DeletePageInput input);
    }
}