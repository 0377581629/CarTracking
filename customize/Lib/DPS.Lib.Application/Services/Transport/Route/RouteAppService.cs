using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.Point;
using DPS.Lib.Application.Shared.Dto.Transport.Route.Details;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using DPS.Lib.Application.Shared.Interface.Transport.Route;
using DPS.Lib.Core.Transport.Route;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.Route
{
    [AbpAuthorize(LibPermissions.Route)]
    public class RouteAppService : ZeroAppServiceBase, IRouteAppService
    {
        private readonly IRepository<Core.Transport.Route.Route> _routeRepository;
        private readonly IRepository<RouteDetail> _routeDetailRepository;
        private readonly IRepository<Core.Transport.Point.Point> _pointRepository;

        public RouteAppService(
            IRepository<Core.Transport.Route.Route> routeRepository,
            IRepository<RouteDetail> routeDetailRepository,
            IRepository<Core.Transport.Point.Point> pointRepository)
        {
            _routeRepository = routeRepository;
            _routeDetailRepository = routeDetailRepository;
            _pointRepository = pointRepository;
        }

        private IQueryable<RouteDto> RouteQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _routeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
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

        private IQueryable<RouteDetailDto> RouteRouteDetailQuery(int routeId)
        {
            var query = from obj in _routeDetailRepository.GetAll()
                    .Where(o => o.RouteId == routeId)
                select new RouteDetailDto()
                {
                    Id = obj.Id,
                    RouteId = obj.RouteId,

                    EndPointId = obj.EndPointId,
                    EndPointCode = obj.EndPoint.Code,
                    EndPointName = obj.EndPoint.Name,
                    
                    Time = obj.Time
                };
            return query;
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

            var routeDetailQuery = RouteRouteDetailQuery(input.Id);

            var output = new GetRouteForEditOutput
            {
                Route = ObjectMapper.Map<CreateOrEditRouteDto>(route)
            };

            output.Route.RouteDetails = await routeDetailQuery.ToListAsync();

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

            input.RouteDetails ??= new List<RouteDetailDto>();

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

            var routeDetailDetails = ObjectMapper.Map<List<RouteDetail>>(input.RouteDetails);
            if (routeDetailDetails.Any())
            {
                foreach (var detail in routeDetailDetails) detail.RouteId = obj.Id;
                await _routeDetailRepository.GetDbContext().BulkSynchronizeAsync(routeDetailDetails,
                    options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.RouteId; });
            }
            else
            {
                await _routeDetailRepository.DeleteAsync(o => o.RouteId == obj.Id);
            }
        }

        [AbpAuthorize(LibPermissions.Route_Edit)]
        protected virtual async Task Update(CreateOrEditRouteDto input)
        {
            if (input.Id.HasValue)
            {
                EntityFrameworkManager.ContextFactory = _ => _routeRepository.GetDbContext();
                var obj = await _routeRepository.FirstOrDefaultAsync(o =>
                    o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);

                var routeDetailDetails = ObjectMapper.Map<List<RouteDetail>>(input.RouteDetails);
                if (routeDetailDetails.Any())
                {
                    foreach (var detail in routeDetailDetails) detail.RouteId = obj.Id;
                    await _routeDetailRepository.GetDbContext().BulkSynchronizeAsync(routeDetailDetails,
                        options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.RouteId; });
                }
                else
                {
                    await _routeDetailRepository.DeleteAsync(o => o.RouteId == obj.Id);
                }
            }
        }

        [AbpAuthorize(LibPermissions.Route_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _routeRepository.FirstOrDefaultAsync(o =>
                o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _routeRepository.DeleteAsync(input.Id);
        }

        public async Task<List<PointDto>> GetPointByIds(List<int> lstPointIds)
        {
            var res = new List<PointDto>();
            var lstPoint = await _pointRepository.GetAll().Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                .ToListAsync();
            foreach (var pointId in lstPointIds)
            {
                var point = lstPoint.FirstOrDefault(o => o.Id == pointId);
                if (point != null)
                    res.Add(new PointDto
                    {
                        TenantId = point.TenantId,
                        Id = point.Id,
                        Code = point.Code,
                        Name = point.Name,
                        Note = point.Note,
                        Address = point.Address,
                        Fax = point.Fax,
                        Latitude = point.Latitude,
                        Longitude = point.Longitude,
                        Phone = point.Phone,
                        ContactPerson = point.ContactPerson,

                        ManagementUnitId = point.ManagementUnitId,
                        ManagementUnitCode = point.ManagementUnit != null ? point.ManagementUnit.Code : "",
                        ManagementUnitName = point.ManagementUnit != null ? point.ManagementUnit.Name : "",

                        PointTypeId = point.PointTypeId,
                        PointTypeCode = point.PointType != null ? point.PointType.Code : "",
                        PointTypeName = point.PointType != null ? point.PointType.Name : "",
                    });
            }

            return res;
        }
    }
}