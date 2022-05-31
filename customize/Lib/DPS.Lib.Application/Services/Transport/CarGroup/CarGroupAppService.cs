using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;
using DPS.Lib.Application.Shared.Interface.Transport.CarGroup;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.CarGroup
{
    [AbpAuthorize(LibPermissions.CarGroup)]
    public class CarGroupAppService: ZeroAppServiceBase, ICarGroupAppService
    {
        private readonly IRepository<Core.Transport.CarGroup.CarGroup> _carGroupRepository;

        public CarGroupAppService(IRepository<Core.Transport.CarGroup.CarGroup> carGroupRepository)
        {
            _carGroupRepository = carGroupRepository;
        }
        
        private IQueryable<CarGroupDto> CarGroupQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _carGroupRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new CarGroupDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Description = obj.Description,
                    IsActive = obj.IsActive,
                    IsSpecialGroup = obj.IsSpecialGroup,
                    City = obj.City
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllCarGroupInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetCarGroupForViewDto>> GetAll(GetAllCarGroupInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = CarGroupQuery(queryInput);

            var pagedAndFilteredCarGroup = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredCarGroup
                select new GetCarGroupForViewDto
                {
                    CarGroup = ObjectMapper.Map<CarGroupDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetCarGroupForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.CarGroup_Edit)]
        public async Task<GetCarGroupForEditOutput> GetCarGroupForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var carGroup = await CarGroupQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetCarGroupForEditOutput
            {
                CarGroup = ObjectMapper.Map<CreateOrEditCarGroupDto>(carGroup)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditCarGroupDto input)
        {
            var res = await _carGroupRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditCarGroupDto input)
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

        [AbpAuthorize(LibPermissions.CarGroup_Create)]
        protected virtual async Task Create(CreateOrEditCarGroupDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.CarGroup.CarGroup>(input);
            obj.TenantId = AbpSession.TenantId;
            await _carGroupRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.CarGroup_Edit)]
        protected virtual async Task Update(CreateOrEditCarGroupDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _carGroupRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.CarGroup_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _carGroupRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _carGroupRepository.DeleteAsync(input.Id);
        }
    }
}