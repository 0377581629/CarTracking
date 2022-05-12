using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Interface.Basic.Rfid;
using DPS.Lib.Core.Basic.Rfid;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;
using Zero.Customize.Dto.Base;

namespace DPS.Lib.Application.Services.Basic.RFID
{
    [AbpAuthorize(LibPermissions.RfidType)]
    public class RfidTypeAppService: ZeroAppServiceBase, IRfidTypeAppService
    {
        private readonly IRepository<RfidType> _rfidTypeRepository;

        public RfidTypeAppService(IRepository<RfidType> rfidTypeRepository)
        {
            _rfidTypeRepository = rfidTypeRepository;
        }
        
        private IQueryable<RfidTypeDto> RfidTypeQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _rfidTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.CardNumber.Contains(input.Filter) || e.SerialNumber.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new RfidTypeDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    CardNumber = obj.CardNumber,
                    CardDer = obj.CardDer,
                    RegisterDate = obj.RegisterDate,
                    UserId = obj.UserId,
                    UserName = obj.User.UserName,
                    IsBlackList = obj.IsBlackList,
                    SerialNumber = obj.SerialNumber,
                    CardType = obj.CardType,
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllRfidTypeInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetRfidTypeForViewDto>> GetAll(GetAllRfidTypeInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = RfidTypeQuery(queryInput);

            var pagedAndFilteredRfidType = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredRfidType
                select new GetRfidTypeForViewDto
                {
                    RfidType = ObjectMapper.Map<RfidTypeDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetRfidTypeForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.RfidType_Edit)]
        public async Task<GetRfidTypeForEditOutput> GetRfidTypeForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var rfidType = await RfidTypeQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetRfidTypeForEditOutput
            {
                RfidType = ObjectMapper.Map<CreateOrEditRfidTypeDto>(rfidType)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditRfidTypeDto input)
        {
            var res = await _rfidTypeRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditRfidTypeDto input)
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

        [AbpAuthorize(LibPermissions.RfidType_Create)]
        protected virtual async Task Create(CreateOrEditRfidTypeDto input)
        {
            var obj = ObjectMapper.Map<RfidType>(input);
            obj.TenantId = AbpSession.TenantId;
            await _rfidTypeRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.RfidType_Edit)]
        protected virtual async Task Update(CreateOrEditRfidTypeDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _rfidTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.RfidType_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _rfidTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _rfidTypeRepository.DeleteAsync(input.Id);
        }
    }
}