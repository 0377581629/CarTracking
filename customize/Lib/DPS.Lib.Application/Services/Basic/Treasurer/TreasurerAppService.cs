using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;
using DPS.Lib.Application.Shared.Interface.Basic.Treasurer;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Basic.Treasurer
{
    [AbpAuthorize(LibPermissions.Treasurer)]
    public class TreasurerAppService: ZeroAppServiceBase,ITreasurerAppService
    {
        private readonly IRepository<Core.Basic.Treasurer.Treasurer> _treasurerRepository;

        public TreasurerAppService(IRepository<Core.Basic.Treasurer.Treasurer> treasurerRepository)
        {
            _treasurerRepository = treasurerRepository;
        }
        
        private IQueryable<TreasurerDto> TreasurerQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _treasurerRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new TreasurerDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Address = obj.Address,
                    Avatar = obj.Avatar,
                    Email = obj.Email,
                    Gender = obj.Gender,
                    PhoneNumber = obj.PhoneNumber,
                    IsStopWorking = obj.IsStopWorking,
                    
                    RfidTypeId = obj.RfidTypeId,
                    RfidTypeCardNumber = obj.RfidType.CardNumber
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllTreasurerInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetTreasurerForViewDto>> GetAll(GetAllTreasurerInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = TreasurerQuery(queryInput);

            var pagedAndFilteredTreasurer = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredTreasurer
                select new GetTreasurerForViewDto
                {
                    Treasurer = ObjectMapper.Map<TreasurerDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetTreasurerForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Treasurer_Edit)]
        public async Task<GetTreasurerForEditOutput> GetTreasurerForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var treasurer = await TreasurerQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetTreasurerForEditOutput
            {
                Treasurer = ObjectMapper.Map<CreateOrEditTreasurerDto>(treasurer)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditTreasurerDto input)
        {
            var res = await _treasurerRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditTreasurerDto input)
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

        [AbpAuthorize(LibPermissions.Treasurer_Create)]
        protected virtual async Task Create(CreateOrEditTreasurerDto input)
        {
            var obj = ObjectMapper.Map<Core.Basic.Treasurer.Treasurer>(input);
            obj.TenantId = AbpSession.TenantId;
            await _treasurerRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Treasurer_Edit)]
        protected virtual async Task Update(CreateOrEditTreasurerDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _treasurerRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Treasurer_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _treasurerRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _treasurerRepository.DeleteAsync(input.Id);
        }
    }
}