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
using DPS.Lib.Application.Shared.Dto.Transport.Car;
using DPS.Lib.Application.Shared.Dto.Transport.Car.Details;
using DPS.Lib.Application.Shared.Interface.Transport.Car;
using DPS.Lib.Core.Transport.Car;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.Car
{
    [AbpAuthorize(LibPermissions.Car)]
    public class CarAppService: ZeroAppServiceBase, ICarAppService
    {
        private readonly IRepository<Core.Transport.Car.Car> _carRepository;
        private readonly IRepository<Camera> _cameraRepository;

        public CarAppService(
            IRepository<Core.Transport.Car.Car> carRepository,
            IRepository<Camera> cameraRepository)
        {
            _carRepository = carRepository;
            _cameraRepository = cameraRepository;
        }
        
        private IQueryable<CarDto> CarQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _carRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.LicensePlate.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new CarDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    LicensePlate = obj.LicensePlate,
                    
                    DeviceId = obj.DeviceId,
                    DeviceCode = obj.Device.Code,
                    DeviceSimCard = obj.Device.SimCard,
                    
                    CarTypeId = obj.CarTypeId,
                    CarTypeCode = obj.CarType.Code,
                    CarTypeName = obj.CarType.Name,
                    
                    CarGroupId = obj.CarGroupId,
                    CarGroupCode = obj.CarGroup.Code,
                    CarGroupName = obj.CarGroup.Name,
                    
                    DriverId = obj.DriverId,
                    DriverCode = obj.Driver.Code,
                    DriverName = obj.Driver.Name,
                    
                    RfidTypeId = obj.RfidTypeId,
                    RfidTypeCode = obj.RfidType.Code,
                    RfidTypeCardNumber = obj.RfidType.CardNumber,
                    
                    Note = obj.Note,
                    FuelType = obj.FuelType,
                    Quota = obj.Quota,
                    SpeedLimit = obj.SpeedLimit
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllCarInput Input { get; init; }
            public int? Id { get; init; }
        }

        private IQueryable<CarCameraDto> CarCameraQuery(int carId)
        {
            var query = from obj in _cameraRepository.GetAll()
                    .Where(o => o.CarId == carId)
                select new CarCameraDto()
                {
                    Id = obj.Id,
                    CarId = obj.CarId,
                    Name = obj.Name,
                    Position = obj.Position,
                    Rotation = obj.Rotation
                };
            return query;
        }

        public async Task<PagedResultDto<GetCarForViewDto>> GetAll(GetAllCarInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = CarQuery(queryInput);

            var pagedAndFilteredCar = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredCar
                select new GetCarForViewDto
                {
                    Car = ObjectMapper.Map<CarDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetCarForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Car_Edit)]
        public async Task<GetCarForEditOutput> GetCarForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var car = await CarQuery(queryInput).FirstOrDefaultAsync();

            var cameraQuery = CarCameraQuery(input.Id);

            var output = new GetCarForEditOutput
            {
                Car = ObjectMapper.Map<CreateOrEditCarDto>(car)
            };

            output.Car.Cameras = await cameraQuery.ToListAsync();
            
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditCarDto input)
        {
            var res = await _carRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditCarDto input)
        {
            input.Code = input.Code.Replace(" ", "");
            
            await ValidateDataInput(input);
            
            input.Cameras ??= new List<CarCameraDto>();

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(LibPermissions.Car_Create)]
        protected virtual async Task Create(CreateOrEditCarDto input)
        {
            EntityFrameworkManager.ContextFactory = _ => _carRepository.GetDbContext();
            var obj = ObjectMapper.Map<Core.Transport.Car.Car>(input);
            obj.TenantId = AbpSession.TenantId;
            await _carRepository.InsertAndGetIdAsync(obj);
            
            var cameraDetails = ObjectMapper.Map<List<Camera>>(input.Cameras);
            if (cameraDetails.Any())
            {
                foreach (var detail in cameraDetails) detail.CarId = obj.Id;
                await _cameraRepository.GetDbContext().BulkSynchronizeAsync(cameraDetails,
                    options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.CarId; });
            }
            else
            {
                await _cameraRepository.DeleteAsync(o => o.CarId == obj.Id);
            }
        }

        [AbpAuthorize(LibPermissions.Car_Edit)]
        protected virtual async Task Update(CreateOrEditCarDto input)
        {
            if (input.Id.HasValue)
            {
                EntityFrameworkManager.ContextFactory = _ => _carRepository.GetDbContext();
                var obj = await _carRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
                
                var cameraDetails = ObjectMapper.Map<List<Camera>>(input.Cameras);
                if (cameraDetails.Any())
                {
                    foreach (var detail in cameraDetails) detail.CarId = obj.Id;
                    await _cameraRepository.GetDbContext().BulkSynchronizeAsync(cameraDetails,
                        options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.CarId; });
                }
                else
                {
                    await _cameraRepository.DeleteAsync(o => o.CarId == obj.Id);
                }
            }
        }

        [AbpAuthorize(LibPermissions.Car_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _carRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _carRepository.DeleteAsync(input.Id);
        }
    }
}