using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Driver;

namespace DPS.Lib.Application.Shared.Interface.Basic.Driver
{
    public interface IDriverAppService: IApplicationService 
    {
        Task<PagedResultDto<GetDriverForViewDto>> GetAll(GetAllDriverInput input);
        
        Task<GetDriverForEditOutput> GetDriverForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDriverDto input);

        Task Delete(EntityDto input);
    }
}