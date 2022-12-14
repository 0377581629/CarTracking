using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using DPS.Lib.Application.Shared.Dto.Basic.Device;
using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;
using DPS.Lib.Application.Shared.Dto.Transport.Car;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;
using DPS.Lib.Application.Shared.Dto.Transport.Driver;
using DPS.Lib.Application.Shared.Dto.Transport.Point;
using DPS.Lib.Application.Shared.Dto.Transport.PointType;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using DPS.Lib.Application.Shared.Interface.Common;
using DPS.Lib.Core.Basic.Device;
using DPS.Lib.Core.Basic.ManagementUnit;
using DPS.Lib.Core.Basic.NetworkProvider;
using DPS.Lib.Core.Basic.Rfid;
using DPS.Lib.Core.Basic.Technician;
using DPS.Lib.Core.Basic.Treasurer;
using DPS.Lib.Core.Transport.Car;
using DPS.Lib.Core.Transport.CarGroup;
using DPS.Lib.Core.Transport.CarType;
using DPS.Lib.Core.Transport.Driver;
using DPS.Lib.Core.Transport.Point;
using DPS.Lib.Core.Transport.PointType;
using DPS.Lib.Core.Transport.Route;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Authorization.Users.Dto;

namespace DPS.Lib.Application.Services.Common
{
    [AbpAuthorize]
    public class LibAppService : ZeroAppServiceBase, ILibAppService
    {
        #region Constructor

        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<RfidType> _rfidTypeRepository;
        private readonly IRepository<NetworkProvider> _networkProviderRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<CarType> _carTypeRepository;
        private readonly IRepository<CarGroup> _carGroupRepository;
        private readonly IRepository<Driver> _driverRepository;
        private readonly IRepository<ManagementUnit> _managementUnitRepository;
        private readonly IRepository<PointType> _pointTypeRepository;
        private readonly IRepository<Car> _carRepository;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Treasurer> _treasurerRepository;
        private readonly IRepository<Technician> _technicianRepository;
        private readonly IRepository<Point> _pointRepository;

        public LibAppService(RoleManager roleManager,
            IRepository<User, long> userRepository,
            IRepository<RfidType> rfidTypeRepository,
            IRepository<NetworkProvider> networkProviderRepository,
            IRepository<Device> deviceRepository,
            IRepository<CarType> carTypeRepository,
            IRepository<CarGroup> carGroupRepository,
            IRepository<Driver> driverRepository,
            IRepository<ManagementUnit> managementUnitRepository, 
            IRepository<PointType> pointTypeRepository,
            IRepository<Car> carRepository,
            IRepository<Route> routeRepository,
            IRepository<Treasurer> treasurerRepository,
            IRepository<Technician> technicianRepository,
            IRepository<Point> pointRepository)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
            _rfidTypeRepository = rfidTypeRepository;
            _networkProviderRepository = networkProviderRepository;
            _deviceRepository = deviceRepository;
            _carTypeRepository = carTypeRepository;
            _carGroupRepository = carGroupRepository;
            _driverRepository = driverRepository;
            _managementUnitRepository = managementUnitRepository;
            _pointTypeRepository = pointTypeRepository;
            _carRepository = carRepository;
            _routeRepository = routeRepository;
            _treasurerRepository = treasurerRepository;
            _technicianRepository = technicianRepository;
            _pointRepository = pointRepository;
        }

        #endregion

        #region User

        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .Where(o => o.IsActive && !o.IsDeleted)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers,
                    u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !string.IsNullOrEmpty(input.Filter),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            return query;
        }

        public async Task<PagedResultDto<UserListDto>> GetPagedUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
            );
        }

        #endregion

        #region RfidType

        private IQueryable<RfidTypeDto> RfidTypeDataQuery(GetAllRfidTypeInput input = null)
        {
            var query = from o in _rfidTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.CardNumber.Contains(input.Filter) ||
                             e.CardDer.Contains(input.Filter))
                select new RfidTypeDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    CardNumber = o.CardNumber,
                    CardDer = o.CardDer,
                    RegisterDate = o.RegisterDate,
                    UserId = o.UserId,
                    UserName = o.User.UserName,
                    IsBlackList = o.IsBlackList,
                    SerialNumber = o.SerialNumber,
                    CardType = o.CardType,
                };
            return query;
        }

        public async Task<List<RfidTypeDto>> GetAllRfidTypes()
        {
            return await RfidTypeDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<RfidTypeDto>> GetPagedRfidTypes(GetAllRfidTypeInput input)
        {
            var objQuery = RfidTypeDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "cardNumber asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<RfidTypeDto>(
                totalCount,
                res
            );
        }

        #endregion

        #region NetworkProvider

        private IQueryable<NetworkProviderDto> NetworkProviderDataQuery(GetAllNetworkProviderInput input = null)
        {
            var query = from o in _networkProviderRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new NetworkProviderDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    AccessPoint = o.AccessPoint,
                    GprsUserName = o.GprsUserName,
                    GprsPassword = o.GprsPassword
                };
            return query;
        }

        public async Task<List<NetworkProviderDto>> GetAllNetworkProviders()
        {
            return await NetworkProviderDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<NetworkProviderDto>> GetPagedNetworkProviders(GetAllNetworkProviderInput input)
        {
            var objQuery = NetworkProviderDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<NetworkProviderDto>(
                totalCount,
                res
            );
        }

        #endregion

        #region Device

        private IQueryable<DeviceDto> DeviceDataQuery(GetAllDeviceInput input = null)
        {
            var query = from o in _deviceRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.SimCard.Contains(input.Filter))
                select new DeviceDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    SimCard = o.SimCard,

                    NetworkProviderId = o.NetworkProviderId,
                    NetworkProviderCode = o.NetworkProvider.Code,
                    NetworkProviderName = o.NetworkProvider.Name,

                    StartDate = o.StartDate,
                    Imei = o.Imei,
                    IsActive = o.IsActive,
                    NeedUpdate = o.NeedUpdate
                };
            return query;
        }

        public async Task<List<DeviceDto>> GetAllDevices()
        {
            return await DeviceDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<DeviceDto>> GetPagedDevices(GetAllDeviceInput input)
        {
            var objQuery = DeviceDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "simCard asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<DeviceDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region CarType

        private IQueryable<CarTypeDto> CarTypeDataQuery(GetAllCarTypeInput input = null)
        {
            var query = from o in _carTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new CarTypeDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Description = o.Description
                };
            return query;
        }

        public async Task<List<CarTypeDto>> GetAllCarTypes()
        {
            return await CarTypeDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<CarTypeDto>> GetPagedCarTypes(GetAllCarTypeInput input)
        {
            var objQuery = CarTypeDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<CarTypeDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region CarGroup

        private IQueryable<CarGroupDto> CarGroupDataQuery(GetAllCarGroupInput input = null)
        {
            var query = from o in _carGroupRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new CarGroupDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Description = o.Description,
                    City = o.City,
                    IsSpecialGroup = o.IsSpecialGroup,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<CarGroupDto>> GetAllCarGroups()
        {
            return await CarGroupDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<CarGroupDto>> GetPagedCarGroups(GetAllCarGroupInput input)
        {
            var objQuery = CarGroupDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<CarGroupDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Car

        private IQueryable<CarDto> CarDataQuery(GetAllCarInput input = null)
        {
            var query = from o in _carRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.LicensePlate.Contains(input.Filter))
                select new CarDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    LicensePlate = o.LicensePlate,
                    
                    DeviceId = o.DeviceId,
                    DeviceCode = o.Device.Code,
                    DeviceSimCard = o.Device.SimCard,
                    
                    CarTypeId = o.CarTypeId,
                    CarTypeCode = o.CarType.Code,
                    CarTypeName = o.CarType.Name,
                    
                    CarGroupId = o.CarGroupId,
                    CarGroupCode = o.CarGroup.Code,
                    CarGroupName = o.CarGroup.Name,
                    
                    DriverId = o.DriverId,
                    DriverCode = o.Driver.Code,
                    DriverName = o.Driver.Name,
                    
                    RfidTypeId = o.RfidTypeId,
                    RfidTypeCode = o.RfidType.Code,
                    RfidTypeCardNumber = o.RfidType.CardNumber,
                    
                    Note = o.Note,
                    FuelType = o.FuelType,
                    Quota = o.Quota,
                    SpeedLimit = o.SpeedLimit
                };
            return query;
        }

        public async Task<List<CarDto>> GetAllCars()
        {
            return await CarDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<CarDto>> GetPagedCars(GetAllCarInput input)
        {
            var objQuery = CarDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "licensePlate asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<CarDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Driver

        private IQueryable<DriverDto> DriverDataQuery(GetAllDriverInput input = null)
        {
            var query = from o in _driverRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new DriverDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Address = o.Address,
                    Avatar = o.Avatar,
                    Email = o.Email,
                    Gender = o.Gender,
                    PhoneNumber = o.PhoneNumber,
                    IsStopWorking = o.IsStopWorking,
                    
                    RfidTypeId = o.RfidTypeId,
                    RfidTypeCardNumber = o.RfidType.CardNumber,
                    
                    DoB = o.DoB,
                    BloodType = o.BloodType,
                    IdNumber = o.IdNumber,
                    DrivingLicense = o.DrivingLicense,
                    DrivingLicenseStartDate = o.DrivingLicenseStartDate,
                    DrivingLicenseEndDate = o.DrivingLicenseEndDate,
                    DriverNumber = o.DriverNumber,
                    
                    DeviceId = o.DeviceId,
                    DeviceCode = o.Device.Code,
                    DeviceSimCard = o.Device.SimCard
                };
            return query;
        }

        public async Task<List<DriverDto>> GetAllDrivers()
        {
            return await DriverDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<DriverDto>> GetPagedDrivers(GetAllDriverInput input)
        {
            var objQuery = DriverDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<DriverDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region ManagementUnit

        private IQueryable<ManagementUnitDto> ManagementUnitDataQuery(GetAllManagementUnitInput input = null)
        {
            var query = from o in _managementUnitRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new ManagementUnitDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note
                };
            return query;
        }

        public async Task<List<ManagementUnitDto>> GetAllManagementUnits()
        {
            return await ManagementUnitDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<ManagementUnitDto>> GetPagedManagementUnits(GetAllManagementUnitInput input)
        {
            var objQuery = ManagementUnitDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<ManagementUnitDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region PointType

        private IQueryable<PointTypeDto> PointTypeDataQuery(GetAllPointTypeInput input = null)
        {
            var query = from o in _pointTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new PointTypeDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note
                };
            return query;
        }

        public async Task<List<PointTypeDto>> GetAllPointTypes()
        {
            return await PointTypeDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<PointTypeDto>> GetPagedPointTypes(GetAllPointTypeInput input)
        {
            var objQuery = PointTypeDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<PointTypeDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Point

        private IQueryable<PointDto> PointDataQuery(GetAllPointInput input = null)
        {
            var query = from o in _pointRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new PointDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Address = o.Address,
                    Fax = o.Fax,
                    Latitude = o.Latitude,
                    Longitude = o.Longitude,
                    Phone = o.Phone,
                    ContactPerson = o.ContactPerson,
                    
                    ManagementUnitId = o.ManagementUnitId,
                    ManagementUnitCode = o.ManagementUnit.Code,
                    ManagementUnitName = o.ManagementUnit.Name,
                    
                    PointTypeId = o.PointTypeId,
                    PointTypeCode = o.PointType.Code,
                    PointTypeName = o.PointType.Name,
                };
            return query;
        }

        public async Task<List<PointDto>> GetAllPoints()
        {
            return await PointDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<PointDto>> GetPagedPoints(GetAllPointInput input)
        {
            var objQuery = PointDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<PointDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Route

        private IQueryable<RouteDto> RouteDataQuery(GetAllRouteInput input = null)
        {
            var query = from o in _routeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new RouteDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    
                    ManagementUnitId = o.ManagementUnitId,
                    ManagementUnitCode = o.ManagementUnit.Code,
                    ManagementUnitName = o.ManagementUnit.Name,
                    
                    ListPoint = o.ListPoint,
                    ListTime = o.ListTime,
                    RouteDetail = o.RouteDetail,
                    IsPermanentRoute = o.IsPermanentRoute,
                    MinuteLate = o.MinuteLate,
                    Range = o.Range,
                    HasConstraintTime = o.HasConstraintTime,
                    RouteType = o.RouteType,
                    EstimateDistance = o.EstimateDistance,
                    EstimatedTime = o.EstimatedTime
                };
            return query;
        }

        public async Task<List<RouteDto>> GetAllRoutes()
        {
            return await RouteDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<RouteDto>> GetPagedRoutes(GetAllRouteInput input)
        {
            var objQuery = RouteDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<RouteDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Treasurer

        private IQueryable<TreasurerDto> TreasurerDataQuery(GetAllTreasurerInput input = null)
        {
            var query = from o in _treasurerRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new TreasurerDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Address = o.Address,
                    Avatar = o.Avatar,
                    Email = o.Email,
                    Gender = o.Gender,
                    PhoneNumber = o.PhoneNumber,
                    IsStopWorking = o.IsStopWorking,
                    
                    RfidTypeId = o.RfidTypeId,
                    RfidTypeCardNumber = o.RfidType.CardNumber
                };
            return query;
        }

        public async Task<List<TreasurerDto>> GetAllTreasurers()
        {
            return await TreasurerDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<TreasurerDto>> GetPagedTreasurers(GetAllTreasurerInput input)
        {
            var objQuery = TreasurerDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<TreasurerDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Technician

        private IQueryable<TechnicianDto> TechnicianDataQuery(GetAllTechnicianInput input = null)
        {
            var query = from o in _technicianRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                        .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                select new TechnicianDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    Address = o.Address,
                    Avatar = o.Avatar,
                    Email = o.Email,
                    Gender = o.Gender,
                    PhoneNumber = o.PhoneNumber,
                    IsStopWorking = o.IsStopWorking,
                    
                    RfidTypeId = o.RfidTypeId,
                    RfidTypeCardNumber = o.RfidType.CardNumber
                };
            return query;
        }

        public async Task<List<TechnicianDto>> GetAllTechnicians()
        {
            return await TechnicianDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<TechnicianDto>> GetPagedTechnicians(GetAllTechnicianInput input)
        {
            var objQuery = TechnicianDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<TechnicianDto>(
                totalCount,
                res
            );
        }

        #endregion
    }
}