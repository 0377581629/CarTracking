using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;

namespace DPS.Lib.Application.Shared.Interface.Basic.Technician
{
    public interface ITechnicianAppService: IApplicationService 
    {
        Task<PagedResultDto<GetTechnicianForViewDto>> GetAll(GetAllTechnicianInput input);
        
        Task<GetTechnicianForEditOutput> GetTechnicianForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTechnicianDto input);

        Task Delete(EntityDto input);
    }
}