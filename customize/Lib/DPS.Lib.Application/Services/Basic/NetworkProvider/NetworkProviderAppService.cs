using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;
using DPS.Lib.Application.Shared.Interface.Basic.NetworkProvider;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Basic.NetworkProvider
{
    [AbpAuthorize(LibPermissions.NetworkProvider)]
    public class NetworkProviderAppService: ZeroAppServiceBase,INetworkProviderAppService
    {
        private readonly IRepository<Core.Basic.NetworkProvider.NetworkProvider> _networkProviderRepository;

        public NetworkProviderAppService(IRepository<Core.Basic.NetworkProvider.NetworkProvider> networkProviderRepository)
        {
            _networkProviderRepository = networkProviderRepository;
        }
        
        private IQueryable<NetworkProviderDto> NetworkProviderQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _networkProviderRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new NetworkProviderDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    AccessPoint = obj.AccessPoint,
                    GprsUserName = obj.GprsUserName,
                    GprsPassword = obj.GprsPassword
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllNetworkProviderInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetNetworkProviderForViewDto>> GetAll(GetAllNetworkProviderInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = NetworkProviderQuery(queryInput);

            var pagedAndFilteredNetworkProvider = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredNetworkProvider
                select new GetNetworkProviderForViewDto
                {
                    NetworkProvider = ObjectMapper.Map<NetworkProviderDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetNetworkProviderForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.NetworkProvider_Edit)]
        public async Task<GetNetworkProviderForEditOutput> GetNetworkProviderForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var networkProvider = await NetworkProviderQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetNetworkProviderForEditOutput
            {
                NetworkProvider = ObjectMapper.Map<CreateOrEditNetworkProviderDto>(networkProvider)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditNetworkProviderDto input)
        {
            var res = await _networkProviderRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditNetworkProviderDto input)
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

        [AbpAuthorize(LibPermissions.NetworkProvider_Create)]
        protected virtual async Task Create(CreateOrEditNetworkProviderDto input)
        {
            var obj = ObjectMapper.Map<Core.Basic.NetworkProvider.NetworkProvider>(input);
            obj.TenantId = AbpSession.TenantId;
            await _networkProviderRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.NetworkProvider_Edit)]
        protected virtual async Task Update(CreateOrEditNetworkProviderDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _networkProviderRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.NetworkProvider_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _networkProviderRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _networkProviderRepository.DeleteAsync(input.Id);
        }
    }
}