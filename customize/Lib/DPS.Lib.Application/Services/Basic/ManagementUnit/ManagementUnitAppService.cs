using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;
using DPS.Lib.Application.Shared.Interface.Basic.ManagementUnit;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Basic.ManagementUnit
{
    [AbpAuthorize(LibPermissions.ManagementUnit)]
    public class ManagementUnitAppService: ZeroAppServiceBase,IManagementUnitAppService
    {
        private readonly IRepository<Core.Basic.ManagementUnit.ManagementUnit> _managementUnitRepository;

        public ManagementUnitAppService(IRepository<Core.Basic.ManagementUnit.ManagementUnit> managementUnitRepository)
        {
            _managementUnitRepository = managementUnitRepository;
        }

        private IQueryable<ManagementUnitDto> ManagementUnitQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _managementUnitRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new ManagementUnitDto
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
            public GetAllManagementUnitInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetManagementUnitForViewDto>> GetAll(GetAllManagementUnitInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = ManagementUnitQuery(queryInput);

            var pagedAndFilteredManagementUnit = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredManagementUnit
                select new GetManagementUnitForViewDto
                {
                    ManagementUnit = ObjectMapper.Map<ManagementUnitDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetManagementUnitForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.ManagementUnit_Edit)]
        public async Task<GetManagementUnitForEditOutput> GetManagementUnitForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var managementUnit = await ManagementUnitQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetManagementUnitForEditOutput
            {
                ManagementUnit = ObjectMapper.Map<CreateOrEditManagementUnitDto>(managementUnit)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditManagementUnitDto input)
        {
            var res = await _managementUnitRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditManagementUnitDto input)
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

        [AbpAuthorize(LibPermissions.ManagementUnit_Create)]
        protected virtual async Task Create(CreateOrEditManagementUnitDto input)
        {
            var obj = ObjectMapper.Map<Core.Basic.ManagementUnit.ManagementUnit>(input);
            obj.TenantId = AbpSession.TenantId;
            await _managementUnitRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.ManagementUnit_Edit)]
        protected virtual async Task Update(CreateOrEditManagementUnitDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _managementUnitRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.ManagementUnit_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _managementUnitRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _managementUnitRepository.DeleteAsync(input.Id);
        }
    }
}