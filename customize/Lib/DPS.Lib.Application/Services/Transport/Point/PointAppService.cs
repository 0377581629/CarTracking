using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.Point;
using DPS.Lib.Application.Shared.Interface.Transport.Point;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.Point
{
    [AbpAuthorize(LibPermissions.Point)]
    public class PointAppService: ZeroAppServiceBase,IPointAppService
    {
        private readonly IRepository<Core.Transport.Point.Point> _pointRepository;

        public PointAppService(IRepository<Core.Transport.Point.Point> pointRepository)
        {
            _pointRepository = pointRepository;
        }
        
        private IQueryable<PointDto> PointQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _pointRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new PointDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Note = obj.Note,
                    Address = obj.Address,
                    Fax = obj.Fax,
                    Latitude = obj.Latitude,
                    Longitude = obj.Longitude,
                    Phone = obj.Phone,
                    ContactPerson = obj.ContactPerson,
                    
                    ManagementUnitId = obj.ManagementUnitId,
                    ManagementUnitCode = obj.ManagementUnit.Code,
                    ManagementUnitName = obj.ManagementUnit.Name,
                    
                    PointTypeId = obj.PointTypeId,
                    PointTypeCode = obj.PointType.Code,
                    PointTypeName = obj.PointType.Name,
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllPointInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetPointForViewDto>> GetAll(GetAllPointInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = PointQuery(queryInput);

            var pagedAndFilteredPoint = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredPoint
                select new GetPointForViewDto
                {
                    Point = ObjectMapper.Map<PointDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPointForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Point_Edit)]
        public async Task<GetPointForEditOutput> GetPointForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var point = await PointQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetPointForEditOutput
            {
                Point = ObjectMapper.Map<CreateOrEditPointDto>(point)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditPointDto input)
        {
            var res = await _pointRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditPointDto input)
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

        [AbpAuthorize(LibPermissions.Point_Create)]
        protected virtual async Task Create(CreateOrEditPointDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.Point.Point>(input);
            obj.TenantId = AbpSession.TenantId;
            await _pointRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Point_Edit)]
        protected virtual async Task Update(CreateOrEditPointDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _pointRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Point_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _pointRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _pointRepository.DeleteAsync(input.Id);
        }
    }
}