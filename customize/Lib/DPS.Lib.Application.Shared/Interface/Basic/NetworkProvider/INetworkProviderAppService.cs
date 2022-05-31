using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;

namespace DPS.Lib.Application.Shared.Interface.Basic.NetworkProvider
{
    public interface INetworkProviderAppService: IApplicationService 
    {
        Task<PagedResultDto<GetNetworkProviderForViewDto>> GetAll(GetAllNetworkProviderInput input);
        
        Task<GetNetworkProviderForEditOutput> GetNetworkProviderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditNetworkProviderDto input);

        Task Delete(EntityDto input);
    }
}