using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization;
using Zero.Customize.Dashboard;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Interfaces;

namespace Zero.Customize
{
    [AbpAuthorize(AppPermissions.DashboardWidget)]
    public class DashboardWidgetAppService : ZeroAppServiceBase, IDashboardWidgetAppService
    {
        private readonly IRepository<DashboardWidget> _dashboardWidgetRepository;
        
        public DashboardWidgetAppService(IRepository<DashboardWidget> dashboardWidgetRepository)
        {
            _dashboardWidgetRepository = dashboardWidgetRepository;
        }

        private IQueryable<DashboardWidgetDto> DashboardWidgetQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _dashboardWidgetRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
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

        private class QueryInput
        {
            public GetAllDashboardWidgetInput Input { get; set; }
            public int? Id { get; set; }
        }
        
        public async Task<List<DashboardWidgetDto>> GetApp()
        {
            var queryInput = new QueryInput()
            {
                
            };
            
            var objQuery = DashboardWidgetQuery(queryInput);
            
            return await objQuery.ToListAsync();
        }
        
        public async Task<PagedResultDto<GetDashboardWidgetForViewDto>> GetAll(GetAllDashboardWidgetInput input)
        {
            var queryInput = new QueryInput()
            {
                Input = input
            };
            
            var objQuery = DashboardWidgetQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetDashboardWidgetForViewDto()
                {
                    DashboardWidget = o
                };

            var totalCount = await objQuery.CountAsync();

            return new PagedResultDto<GetDashboardWidgetForViewDto>(
                totalCount,
                await objs.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.DashboardWidget_Edit)]
        public async Task<GetDashboardWidgetForEditOutput> GetDashboardWidgetForEdit(EntityDto input)
        {
            var queryInput = new QueryInput()
            {
                Id = input.Id
            };
            
            var objQuery = DashboardWidgetQuery(queryInput);
            
            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetDashboardWidgetForEditOutput
            {
                DashboardWidget = ObjectMapper.Map<CreateOrEditDashboardWidgetDto>(obj)
            };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDashboardWidgetDto input)
        {
            if (string.IsNullOrEmpty(input.WidgetId))
                input.WidgetId = StringHelper.Identity();
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.DashboardWidget_Create)]
        protected virtual async Task Create(CreateOrEditDashboardWidgetDto input)
        {
            var obj = ObjectMapper.Map<DashboardWidget>(input);
            await _dashboardWidgetRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(AppPermissions.DashboardWidget_Edit)]
        protected virtual async Task Update(CreateOrEditDashboardWidgetDto input)
        {
            var obj = await _dashboardWidgetRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, obj);
        }

        [AbpAuthorize(AppPermissions.DashboardWidget_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _dashboardWidgetRepository.DeleteAsync(input.Id);
        }
    }
}