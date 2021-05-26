using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zero;
using Zero.Configuration;
using Zero.Customize.Dashboard;
using Zero.Customize.Dto.Dashboard.Config;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Dto.Dashboard.Function;
using Zero.Customize.Interfaces;

namespace Zero.Customize
{
    [AbpAuthorize]
    public class DashboardAppService : ZeroAppServiceBase, IDashboardAppService
    {
        private readonly IZeroAppService _zeroAppService;
        private readonly IRepository<DashboardWidget> _dashboardWidgetRepository;
        private readonly IRepository<EditionDashboardWidget> _editionDashboardWidgetRepository;
        private readonly IRepository<RoleDashboardWidget> _roleDashboardWidgetRepository;
        private readonly ISettingStore _settingStore;
        public DashboardAppService(IRepository<DashboardWidget> dashboardWidgetRepository, IRepository<EditionDashboardWidget> editionDashboardWidgetRepository, IRepository<RoleDashboardWidget> roleDashboardWidgetRepository, IZeroAppService zeroAppService, ISettingStore settingStore)
        {
            _dashboardWidgetRepository = dashboardWidgetRepository;
            _editionDashboardWidgetRepository = editionDashboardWidgetRepository;
            _roleDashboardWidgetRepository = roleDashboardWidgetRepository;
            _zeroAppService = zeroAppService;
            _settingStore = settingStore;
        }

        private IQueryable<DashboardWidgetDto> DashboardWidgetQuery()
        {
            var query = from o in _dashboardWidgetRepository.GetAll()
                select new DashboardWidgetDto()
                {
                    Id = o.Id,
                    WidgetId = o.WidgetId,
                    Name = o.Name,
                    Description = o.Description,
                    
                    Width = o.Width,
                    Height = o.Height,
                    PositionX = o.PositionX,
                    PositionY = o.PositionY,

                    ViewName = o.ViewName,
                    JsPath = o.JsPath,
                    CssPath = o.CssPath,
                    
                    Filters = o.Filters,
                    IsDefault = o.IsDefault
                };

            return query;
        }

        public async Task<List<DashboardWidgetDto>> GetAllDashboardWidget()
        {
            return await DashboardWidgetQuery().ToListAsync();
        }
        
        public async Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByEdition(int editionId)
        {
            var grantedByEdition = await _editionDashboardWidgetRepository.GetAllListAsync(o => o.EditionId == editionId);
            var allWidgets = await DashboardWidgetQuery().ToListAsync();
            return allWidgets.Where(o=>grantedByEdition.Any(g=>g.DashboardWidgetId == o.Id)).ToList();
        }
        
        public async Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByRole(int roleId)
        {
            var grantedByEdition = await _roleDashboardWidgetRepository.GetAllListAsync(o => o.RoleId == roleId);
            var allWidgets = await DashboardWidgetQuery().ToListAsync();
            return allWidgets.Where(o=>grantedByEdition.Any(g=>g.DashboardWidgetId == o.Id)).ToList();
        }
        
        public async Task<List<DashboardWidgetDto>> GetAllDashboardWidgetByRoles(List<int> roleIds)
        {
            var grantedByEdition = await _roleDashboardWidgetRepository.GetAllListAsync(o => roleIds.Contains(o.RoleId));
            var allWidgets = await DashboardWidgetQuery().ToListAsync();
            return allWidgets.Where(o=>grantedByEdition.Any(g=>g.DashboardWidgetId == o.Id)).ToList();
        }
        
        private static string SettingName => "App.DashboardCustomization.Configuration.Mvc";
        
        public async Task<Dto.Dashboard.Config.Dashboard> GetUserDashboard()
        {
            return GetDashboard(await GetDashboardFromSettings());
        }
        
        public async Task SavePage(SavePageInput input)
        {
            var dashboards = await GetDashboardFromSettings();
            var dashboard = GetDashboard(dashboards);

            foreach (var inputPage in input.Pages)
            {
                var page = dashboard.Pages.FirstOrDefault(p => p.Id == inputPage.Id);
                var pageIndex = dashboard.Pages.IndexOf(page);

                dashboard.Pages.RemoveAt(pageIndex);

                if (page != null)
                {
                    inputPage.Name = page.Name;
                    dashboard.Pages.Insert(pageIndex, inputPage);
                }
            }

            await SaveSetting(dashboards);
        }
        
        public async Task RenamePage(RenamePageInput input)
        {
            var dashboards = await GetDashboardFromSettings();
            var dashboard = GetDashboard(dashboards);

            var page = dashboard.Pages.FirstOrDefault(p => p.Id == input.Id);
            if (page == null)
            {
                return;
            }

            page.Name = input.Name;

            await SaveSetting(dashboards);
        }

        public async Task<AddNewPageOutput> AddNewPage(AddNewPageInput input)
        {
            var dashboards = await GetDashboardFromSettings();
            var dashboard = GetDashboard(dashboards);

            var page = new Page
            {
                Name = input.Name,
                Widgets = new List<DashboardWidgetDto>(),
            };

            dashboard.Pages.Add(page);
            await SaveSetting(dashboards);

            return new AddNewPageOutput { PageId = page.Id };
        }
        
        public async Task DeletePage(DeletePageInput input)
        {
            var dashboards = await GetDashboardFromSettings();
            var dashboard = GetDashboard(dashboards);
            dashboard.Pages.RemoveAll(p => p.Id == input.Id);
            await SaveSetting(dashboards);
        }

        public async Task<DashboardWidgetDto> AddWidget(AddWidgetInput input)
        {
            var dashboards = await GetDashboardFromSettings();
            var dashboard = GetDashboard(dashboards);

            var page = dashboard.Pages.Single(p => p.Id == input.PageId);

            var widget = new DashboardWidgetDto
            {
                WidgetId = input.WidgetId,
                Height = input.Height,
                Width = input.Width,
                PositionX = 0,
                PositionY = CalculatePositionY(page.Widgets)
            };

            page.Widgets.Add(widget);

            await SaveSetting(dashboards);
            return widget;
        }
        
        private Dto.Dashboard.Config.Dashboard GetDashboard(List<Dto.Dashboard.Config.Dashboard> dashboards)
        {
            var dashboard = dashboards.FirstOrDefault();
            if (dashboard == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", "Dashboard"));
            }

            return dashboard;
        }
        
        private byte CalculatePositionY(List<DashboardWidgetDto> widgets)
        {
            if (widgets == null || !widgets.Any())
            {
                return 0;
            }

            return (byte)widgets.Max(w => w.PositionY + w.Height);
        }
        
        private async Task<List<Dto.Dashboard.Config.Dashboard>> GetDashboardFromSettings()
        {
            var allUserSettings = await _settingStore.GetAllListAsync(AbpSession.TenantId, AbpSession.UserId);
            if (!allUserSettings.Any(o => o.Name.Equals(SettingName)))
            {
                var rolesIds = await _zeroAppService.GetCurrentRoleIds();
                var defaultWidgets = (await GetAllDashboardWidgetByRoles(rolesIds)).Where(o=>o.IsDefault).ToList();
                return new List<Dto.Dashboard.Config.Dashboard>()
                {
                    new()
                    {
                        DashboardName = "Dashboard",
                        Pages = new List<Page>()
                        {
                            new(ZeroDashboardCustomizationConsts.DefaultDashboardPageId)
                            {
                                Name = ZeroDashboardCustomizationConsts.DefaultPageName,
                                Widgets = defaultWidgets.Any()? defaultWidgets : new List<DashboardWidgetDto>()
                            }
                        }
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<Dto.Dashboard.Config.Dashboard>>(allUserSettings.First(o=>o.Name == SettingName).Value);
        }
        
        private async Task SaveSetting(List<Dto.Dashboard.Config.Dashboard> dashboards)
        {
            var value = JsonConvert.SerializeObject(dashboards);
            
            var allUserSettings = await _settingStore.GetAllListAsync(AbpSession.TenantId, AbpSession.UserId);
            var currentSetting = allUserSettings.FirstOrDefault(o => o.Name.Equals(SettingName));
            if (currentSetting == null)
            {
                await _settingStore.CreateAsync(new SettingInfo(AbpSession.TenantId, AbpSession.UserId, SettingName,
                    value));
            }
            else
            {
                currentSetting.Value = value;
                await _settingStore.UpdateAsync(currentSetting);
            }
        }
    }
}