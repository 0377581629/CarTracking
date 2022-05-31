using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.Driver;
using DPS.Lib.Application.Shared.Interface.Transport.Driver;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.Driver
{
    [AbpAuthorize(LibPermissions.Driver)]
    public class DriverAppService: ZeroAppServiceBase,IDriverAppService
    {
        private readonly IRepository<Core.Transport.Driver.Driver> _driverRepository;

        public DriverAppService(IRepository<Core.Transport.Driver.Driver> driverRepository)
        {
            _driverRepository = driverRepository;
        }
        
        private IQueryable<DriverDto> DriverQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _driverRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new DriverDto
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
                    RfidTypeCardNumber = obj.RfidType.CardNumber,
                    
                    DoB = obj.DoB,
                    BloodType = obj.BloodType,
                    IdNumber = obj.IdNumber,
                    DrivingLicense = obj.DrivingLicense,
                    DrivingLicenseStartDate = obj.DrivingLicenseStartDate,
                    DrivingLicenseEndDate = obj.DrivingLicenseEndDate,
                    DriverNumber = obj.DriverNumber,
                    
                    DeviceId = obj.DeviceId,
                    DeviceCode = obj.Device.Code,
                    DeviceSimCard = obj.Device.SimCard
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllDriverInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetDriverForViewDto>> GetAll(GetAllDriverInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = DriverQuery(queryInput);

            var pagedAndFilteredDriver = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredDriver
                select new GetDriverForViewDto
                {
                    Driver = ObjectMapper.Map<DriverDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetDriverForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Driver_Edit)]
        public async Task<GetDriverForEditOutput> GetDriverForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var driver = await DriverQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetDriverForEditOutput
            {
                Driver = ObjectMapper.Map<CreateOrEditDriverDto>(driver)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditDriverDto input)
        {
            var res = await _driverRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditDriverDto input)
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

        [AbpAuthorize(LibPermissions.Driver_Create)]
        protected virtual async Task Create(CreateOrEditDriverDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.Driver.Driver>(input);
            obj.TenantId = AbpSession.TenantId;
            await _driverRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Driver_Edit)]
        protected virtual async Task Update(CreateOrEditDriverDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _driverRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Driver_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _driverRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _driverRepository.DeleteAsync(input.Id);
        }
    }
}