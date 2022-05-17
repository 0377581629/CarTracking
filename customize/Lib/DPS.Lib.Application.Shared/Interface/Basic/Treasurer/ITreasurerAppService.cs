using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;

namespace DPS.Lib.Application.Shared.Interface.Basic.Treasurer
{
    public interface ITreasurerAppService: IApplicationService 
    {
        Task<PagedResultDto<GetTreasurerForViewDto>> GetAll(GetAllTreasurerInput input);
        
        Task<GetTreasurerForEditOutput> GetTreasurerForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTreasurerDto input);

        Task Delete(EntityDto input);
    }
}