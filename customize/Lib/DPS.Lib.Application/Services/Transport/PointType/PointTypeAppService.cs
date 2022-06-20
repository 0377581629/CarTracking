using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.PointType;
using DPS.Lib.Application.Shared.Interface.Transport.PointType;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.PointType
{
    [AbpAuthorize(LibPermissions.PointType)]
    public class PointTypeAppService: ZeroAppServiceBase,IPointTypeAppService
    {
        private readonly IRepository<Core.Transport.PointType.PointType> _pointTypeRepository;

        public PointTypeAppService(IRepository<Core.Transport.PointType.PointType> pointTypeRepository)
        {
            _pointTypeRepository = pointTypeRepository;
        }
        
        private IQueryable<PointTypeDto> PointTypeQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _pointTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new PointTypeDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Note = obj.Note
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllPointTypeInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetPointTypeForViewDto>> GetAll(GetAllPointTypeInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = PointTypeQuery(queryInput);

            var pagedAndFilteredPointType = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredPointType
                select new GetPointTypeForViewDto
                {
                    PointType = ObjectMapper.Map<PointTypeDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPointTypeForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.PointType_Edit)]
        public async Task<GetPointTypeForEditOutput> GetPointTypeForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var pointType = await PointTypeQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetPointTypeForEditOutput
            {
                PointType = ObjectMapper.Map<CreateOrEditPointTypeDto>(pointType)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditPointTypeDto input)
        {
            var res = await _pointTypeRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditPointTypeDto input)
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

        [AbpAuthorize(LibPermissions.PointType_Create)]
        protected virtual async Task Create(CreateOrEditPointTypeDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.PointType.PointType>(input);
            obj.TenantId = AbpSession.TenantId;
            await _pointTypeRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.PointType_Edit)]
        protected virtual async Task Update(CreateOrEditPointTypeDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _pointTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.PointType_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _pointTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _pointTypeRepository.DeleteAsync(input.Id);
        }
    }
}