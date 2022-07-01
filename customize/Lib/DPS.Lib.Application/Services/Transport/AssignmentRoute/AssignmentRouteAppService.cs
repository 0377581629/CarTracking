using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute;
using DPS.Lib.Application.Shared.Interface.Transport.AssignmentRoute;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.AssignmentRoute
{
    [AbpAuthorize(LibPermissions.AssignmentRoute)]
    public class AssignmentRouteAppService: ZeroAppServiceBase , IAssignmentRouteAppService
    {
        private readonly IRepository<Core.Transport.AssignmentRoute.AssignmentRoute> _assignmentRouteRepository;

        public AssignmentRouteAppService(IRepository<Core.Transport.AssignmentRoute.AssignmentRoute> assignmentRouteRepository)
        {
            _assignmentRouteRepository = assignmentRouteRepository;
        }
        
        private IQueryable<AssignmentRouteDto> AssignmentRouteQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _assignmentRouteRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new AssignmentRouteDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    
                    ManagementUnitId = obj.ManagementUnitId,
                    ManagementUnitCode = obj.ManagementUnit.Code,
                    ManagementUnitName = obj.ManagementUnit.Name,
                    
                    CarId = obj.CarId,
                    CarCode = obj.Car.Code,
                    CarLicensePlate = obj.Car.LicensePlate,
                    
                    DriverId = obj.DriverId,
                    DriverCode = obj.Driver.Code,
                    DriverName = obj.Driver.Name,
                    
                    RouteId = obj.RouteId,
                    RouteCode = obj.Route.Code,
                    RouteName = obj.Route.Name,
                    
                    TreasurerId = obj.TreasurerId,
                    TreasurerCode = obj.Treasurer.Code,
                    TreasurerName = obj.Treasurer.Name,
                    
                    TechnicianId = obj.TechnicianId,
                    TechnicianCode = obj.Technician.Code,
                    TechnicianName = obj.Technician.Name,
                    
                    Guard = obj.Guard,
                    StartDate = obj.StartDate,
                    EndDate = obj.EndDate,
                    DayOfWeeks = obj.DayOfWeeks,
                    IsAssignment = obj.IsAssignment
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllAssignmentRouteInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetAssignmentRouteForViewDto>> GetAll(GetAllAssignmentRouteInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = AssignmentRouteQuery(queryInput);

            var pagedAndFilteredAssignmentRoute = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredAssignmentRoute
                select new GetAssignmentRouteForViewDto
                {
                    AssignmentRoute = ObjectMapper.Map<AssignmentRouteDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetAssignmentRouteForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.AssignmentRoute_Edit)]
        public async Task<GetAssignmentRouteForEditOutput> GetAssignmentRouteForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var assignmentRoute = await AssignmentRouteQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetAssignmentRouteForEditOutput
            {
                AssignmentRoute = ObjectMapper.Map<CreateOrEditAssignmentRouteDto>(assignmentRoute)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditAssignmentRouteDto input)
        {
            var res = await _assignmentRouteRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditAssignmentRouteDto input)
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

        [AbpAuthorize(LibPermissions.AssignmentRoute_Create)]
        protected virtual async Task Create(CreateOrEditAssignmentRouteDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.AssignmentRoute.AssignmentRoute>(input);
            obj.TenantId = AbpSession.TenantId;
            await _assignmentRouteRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.AssignmentRoute_Edit)]
        protected virtual async Task Update(CreateOrEditAssignmentRouteDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _assignmentRouteRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.AssignmentRoute_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _assignmentRouteRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _assignmentRouteRepository.DeleteAsync(input.Id);
        }
    }
}