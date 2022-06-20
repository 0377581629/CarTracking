using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using DPS.Lib.Application.Shared.Interface.Transport.Route;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.Route
{
    [AbpAuthorize(LibPermissions.Route)]
    public class RouteAppService: ZeroAppServiceBase, IRouteAppService
    {
        private readonly IRepository<Core.Transport.Route.Route> _routeRepository;

        public RouteAppService(IRepository<Core.Transport.Route.Route> routeRepository)
        {
            _routeRepository = routeRepository;
        }
        
        private IQueryable<RouteDto> RouteQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _routeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new RouteDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    
                    ManagementUnitId = obj.ManagementUnitId,
                    ManagementUnitCode = obj.ManagementUnit.Code,
                    ManagementUnitName = obj.ManagementUnit.Name,
                    
                    ListPoint = obj.ListPoint,
                    ListTime = obj.ListTime,
                    RouteDetail = obj.RouteDetail,
                    IsPermanentRoute = obj.IsPermanentRoute,
                    MinuteLate = obj.MinuteLate,
                    Range = obj.Range,
                    HasConstraintTime = obj.HasConstraintTime,
                    RouteType = obj.RouteType,
                    EstimateDistance = obj.EstimateDistance,
                    EstimatedTime = obj.EstimatedTime
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllRouteInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetRouteForViewDto>> GetAll(GetAllRouteInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = RouteQuery(queryInput);

            var pagedAndFilteredRoute = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredRoute
                select new GetRouteForViewDto
                {
                    Route = ObjectMapper.Map<RouteDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetRouteForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Route_Edit)]
        public async Task<GetRouteForEditOutput> GetRouteForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var route = await RouteQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetRouteForEditOutput
            {
                Route = ObjectMapper.Map<CreateOrEditRouteDto>(route)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditRouteDto input)
        {
            var res = await _routeRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditRouteDto input)
        {
            input.Code = input.Code.Replace(" ", "");
            
            await ValidateDataInput(input);

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(LibPermissions.Route_Create)]
        protected virtual async Task Create(CreateOrEditRouteDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.Route.Route>(input);
            obj.TenantId = AbpSession.TenantId;
            await _routeRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Route_Edit)]
        protected virtual async Task Update(CreateOrEditRouteDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _routeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Route_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _routeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _routeRepository.DeleteAsync(input.Id);
        }
    }
}