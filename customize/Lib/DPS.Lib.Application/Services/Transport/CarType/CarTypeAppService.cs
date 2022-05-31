using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;
using DPS.Lib.Application.Shared.Interface.Transport.CarType;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Transport.CarType
{
    [AbpAuthorize(LibPermissions.CarType)]
    public class CarTypeAppService: ZeroAppServiceBase, ICarTypeAppService
    {
        private readonly IRepository<Core.Transport.CarType.CarType> _carTypeRepository;

        public CarTypeAppService(IRepository<Core.Transport.CarType.CarType> carTypeRepository)
        {
            _carTypeRepository = carTypeRepository;
        }
        
        private IQueryable<CarTypeDto> CarTypeQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _carTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new CarTypeDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Description = obj.Description
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllCarTypeInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetCarTypeForViewDto>> GetAll(GetAllCarTypeInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = CarTypeQuery(queryInput);

            var pagedAndFilteredCarType = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredCarType
                select new GetCarTypeForViewDto
                {
                    CarType = ObjectMapper.Map<CarTypeDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetCarTypeForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.CarType_Edit)]
        public async Task<GetCarTypeForEditOutput> GetCarTypeForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var carType = await CarTypeQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetCarTypeForEditOutput
            {
                CarType = ObjectMapper.Map<CreateOrEditCarTypeDto>(carType)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditCarTypeDto input)
        {
            var res = await _carTypeRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditCarTypeDto input)
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

        [AbpAuthorize(LibPermissions.CarType_Create)]
        protected virtual async Task Create(CreateOrEditCarTypeDto input)
        {
            var obj = ObjectMapper.Map<Core.Transport.CarType.CarType>(input);
            obj.TenantId = AbpSession.TenantId;
            await _carTypeRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.CarType_Edit)]
        protected virtual async Task Update(CreateOrEditCarTypeDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _carTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.CarType_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _carTypeRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _carTypeRepository.DeleteAsync(input.Id);
        }
    }
}