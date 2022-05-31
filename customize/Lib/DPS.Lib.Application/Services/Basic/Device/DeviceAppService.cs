using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.Device;
using DPS.Lib.Application.Shared.Interface.Basic.Device;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Basic.Device
{
    [AbpAuthorize(LibPermissions.Device)]
    public class DeviceAppService: ZeroAppServiceBase,IDeviceAppService
    {
        private readonly IRepository<Core.Basic.Device.Device> _deviceRepository;

        public DeviceAppService(IRepository<Core.Basic.Device.Device> deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }
        
        private IQueryable<DeviceDto> DeviceQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _deviceRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.SimCard.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new DeviceDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    SimCard = obj.SimCard,
                    
                    NetworkProviderId = obj.NetworkProviderId,
                    NetworkProviderCode = obj.NetworkProvider.Code,
                    NetworkProviderName = obj.NetworkProvider.Name,
                    
                    StartDate = obj.StartDate,
                    Imei = obj.Imei,
                    IsActive = obj.IsActive,
                    NeedUpdate = obj.NeedUpdate
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllDeviceInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetDeviceForViewDto>> GetAll(GetAllDeviceInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = DeviceQuery(queryInput);

            var pagedAndFilteredDevice = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredDevice
                select new GetDeviceForViewDto
                {
                    Device = ObjectMapper.Map<DeviceDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetDeviceForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Device_Edit)]
        public async Task<GetDeviceForEditOutput> GetDeviceForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var device = await DeviceQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetDeviceForEditOutput
            {
                Device = ObjectMapper.Map<CreateOrEditDeviceDto>(device)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditDeviceDto input)
        {
            var res = await _deviceRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditDeviceDto input)
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

        [AbpAuthorize(LibPermissions.Device_Create)]
        protected virtual async Task Create(CreateOrEditDeviceDto input)
        {
            var obj = ObjectMapper.Map<Core.Basic.Device.Device>(input);
            obj.TenantId = AbpSession.TenantId;
            await _deviceRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Device_Edit)]
        protected virtual async Task Update(CreateOrEditDeviceDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _deviceRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Device_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _deviceRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _deviceRepository.DeleteAsync(input.Id);
        }
    }
}