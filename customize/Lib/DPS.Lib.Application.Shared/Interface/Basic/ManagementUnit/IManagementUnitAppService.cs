using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;

namespace DPS.Lib.Application.Shared.Interface.Basic.ManagementUnit
{
    public interface IManagementUnitAppService: IApplicationService 
    {
        Task<PagedResultDto<GetManagementUnitForViewDto>> GetAll(GetAllManagementUnitInput input);
        
        Task<GetManagementUnitForEditOutput> GetManagementUnitForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditManagementUnitDto input);

        Task Delete(EntityDto input);
    }
}